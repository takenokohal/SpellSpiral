using System;
using System.Collections.Generic;
using System.Linq;
using Audio;
using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Others;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;

namespace Battle.Character
{
    public abstract class CharacterBase : SerializedMonoBehaviour, IAttackHittable
    {
        [SerializeField, ValueDropdown(nameof(GetCharacterKeys))]
        private string characterKey;

        public string CharacterKey => characterKey;
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }

        public bool AnimatorIsNull { get; private set; }
        public CharacterRotation CharacterRotation { get; private set; }

        public CharacterData CharacterData { get; private set; }

        [Inject] protected AttackHitEffectFactory AttackHitEffectFactory { get; private set; }
        [Inject] protected AttackDatabase AttackDatabase { get; private set; }
        [Inject] protected CharacterDatabase CharacterDatabase { get; private set; }
        [Inject] protected AllCharacterManager AllCharacterManager { get; private set; }
        [Inject] protected BattleLoop BattleLoop { get; private set; }
        [Inject] protected MagicCircleFactory MagicCircleFactory { get; private set; }

        [Inject] protected CharacterFactory CharacterFactory { get; private set; }
        [Inject] protected ReadyEffectFactory ReadyEffectFactory { get; private set; }
        [Inject] protected CharacterCamera CharacterCamera { get; private set; }
        [Inject] protected CameraSwitcher CameraSwitcher { get; private set; }


        protected CharacterBase Master { get; private set; }


        private Vector3 _animatorLocalPosition;
        private readonly ReactiveProperty<float> _currentLife = new();

        public float CurrentLife
        {
            get => _currentLife.Value;
            set
            {
                _currentLife.Value = Mathf.Clamp(value, 0, CharacterData.Life);
                if (value <= 0)
                    IsDead = true;
            }
        }

        public IObservable<float> CurrentLifeObservable => _currentLife.TakeUntilDestroy(this);

        private readonly ReactiveProperty<bool> _isDead = new();

        public bool IsDead
        {
            get => _isDead.Value;
            private set => _isDead.Value = value;
        }


        public IObservable<Unit> OnDeadObservable() =>
            _isDead.Where(value => value).AsUnitObservable().TakeUntilDestroy(this);

        public void AcquiredInject(
            CharacterBase master,
            AttackHitEffectFactory attackHitEffectFactory,
            AttackDatabase attackDatabase,
            CharacterDatabase characterDatabase,
            AllCharacterManager allCharacterManager,
            BattleLoop battleLoop,
            MagicCircleFactory magicCircleFactory,
            CharacterFactory characterFactory,
            ReadyEffectFactory readyEffectFactory,
            CharacterCamera characterCamera,
            CameraSwitcher cameraSwitcher)
        {
            Master = master;
            
            AttackHitEffectFactory = attackHitEffectFactory;
            AttackDatabase = attackDatabase;
            CharacterDatabase = characterDatabase;
            AllCharacterManager = allCharacterManager;
            BattleLoop = battleLoop;
            MagicCircleFactory = magicCircleFactory;
            CharacterFactory = characterFactory;
            ReadyEffectFactory = readyEffectFactory;
            CharacterCamera = characterCamera;
            CameraSwitcher = cameraSwitcher;
        }


        public bool IsInitialized { get; private set; }

        public UniTask WaitUntilInitialize() =>
            UniTask.WaitUntil(() => IsInitialized, cancellationToken: destroyCancellationToken);

        protected virtual void InitializeFunction()
        {
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            if (Animator == null)
                AnimatorIsNull = true;

            CharacterRotation = new CharacterRotation(transform)
            {
                IsStop = true
            };
            
            BattleLoop.Event.Where(value => value == BattleEvent.BattleStart).Take(1)
                .Subscribe(_ => CharacterRotation.IsStop = false).AddTo(this);

            CharacterData = CharacterDatabase.Find(characterKey);

            AllCharacterManager.RegisterCharacter(this);

            _animatorLocalPosition = AnimatorIsNull ? Vector3.zero : Animator.transform.localPosition;
            CurrentLife = CharacterData.Life;

            InitializeFunction();

            IsInitialized = true;
        }


        //abstractにするかも
        bool IAttackHittable.CheckHit(AttackHitController attackHitController)
        {
            if (!IsInitialized)
                return false;

            if (IsDead)
                return false;

            if (!ChickHitChild(attackHitController))
                return false;
            var key = attackHitController.AttackKey;
            var data = AttackDatabase.Find(key);
            var attackOwner = data.OwnerType;
            return GetOwnerType() != attackOwner;
        }

        protected virtual bool ChickHitChild(AttackHitController attackHitController)
        {
            return true;
        }

        void IAttackHittable.OnAttacked(AttackHitController attackHitController)
        {
            var attackData = AttackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
            {
                CurrentLife -= CalcDamage(attackHitController);
            }

            AllAudioManager.PlaySe("Hit");
            CharacterCamera.ImpulseSource.GenerateImpulse();
            AttackHitEffectFactory.Create(transform.position, transform.rotation).Forget();

            if (!AnimatorIsNull)
            {
                Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                    .OnComplete(() => Animator.transform.localPosition = _animatorLocalPosition);
                Animator.Play("OnDamage", 0, 0);
            }

            OnAttackedChild(attackHitController);
        }

        protected abstract float CalcDamage(AttackHitController attackHitController);

        protected virtual void OnAttackedChild(AttackHitController attackHitController)
        {
        }

        public OwnerType GetOwnerType()
        {
            return CharacterData.OwnerType;
        }

        private static IEnumerable<string> GetCharacterKeys()
        {
#if UNITY_EDITOR
            var db = CharacterDatabase.LoadOnEditor();
            return db.CharacterDictionary.Select(value => value.Key);

#else
            return null;
#endif
        }
    }
}