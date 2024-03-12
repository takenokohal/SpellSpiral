using Battle.Attack;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Others;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerCore : MonoBehaviour, IAttackHittable
    {
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInput PlayerInput { get; private set; }
        public CharacterRotation CharacterRotation { get; private set; }

        [SerializeField] private Transform center;
        [Inject] private readonly CinemachineImpulseSource _cinemachineImpulseSource;
        [Inject] private readonly AttackHitEffectFactory _attackHitEffectFactory;
        [Inject] private readonly AttackDatabase _attackDatabase;
        [Inject] private readonly GameLoop _gameLoop;
        [Inject] private readonly PlayerConstData _playerConstData;
        public PlayerConstData PlayerConstData => _playerConstData;

        private static readonly int OnDamagedAnimKey = Animator.StringToHash("OnDamaged");

        public Transform Center => center;

        public bool IsInitialized { get; private set; }

        public bool IsBattleStarted { get; private set; }
        public UniTask WaitUntilInitialize() => UniTask.WaitWhile(() => !IsInitialized);

        public UniTask WaitUntilBattleStart() => UniTask.WaitUntil(() => IsBattleStarted);

        public PlayerParameter PlayerParameter { get; } = new();

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            PlayerInput = GetComponent<PlayerInput>();
            CharacterRotation = GetComponent<CharacterRotation>();

            PlayerParameter.LifeObservable.Where(value => value <= 0).Subscribe(value =>
            {
                _gameLoop.SendEvent(GameLoop.GameEvent.Lose);
                OnDead();
            }).AddTo(this);

            _gameLoop.Event.Where(value => value == GameLoop.GameEvent.Win)
                .Subscribe(_ => PlayerParameter.Invincible = true).AddTo(this);


            _gameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => IsBattleStarted = true);

            IsInitialized = true;
        }


        void IAttackHittable.OnAttacked(AttackHitController attackHitController)
        {
            if (PlayerParameter.Invincible)
                return;


            var attackData = _attackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
                PlayerParameter.Life -= attackData.Damage;

            _cinemachineImpulseSource.GenerateImpulse();
            _attackHitEffectFactory.Create(Center.position, Center.rotation);

            Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                .OnComplete(() => Animator.transform.localPosition = Vector3.zero);
            Animator.SetTrigger(OnDamagedAnimKey);
        }

        private void OnDead()
        {
            var targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            var deadObj = new GameObject("DeadObject")
            {
                transform =
                {
                    position = center.position
                }
            };

            targetGroup.AddMember(deadObj.transform, 1, 0);


            gameObject.SetActive(false);
        }

        public OwnerType GetOwnerType()
        {
            return OwnerType.Player;
        }

        public Vector2 GetDirectionToPlayer(Vector2 from)
        {
            return ((Vector2)transform.position - from).normalized;
        }
    }
}