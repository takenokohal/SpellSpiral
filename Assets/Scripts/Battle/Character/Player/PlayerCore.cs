using Battle.Attack;
using Cinemachine;
using Databases;
using DG.Tweening;
using Others;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerCore : CharacterBase
    {
        public PlayerInput PlayerInput { get; private set; }

        [Inject] private readonly PlayerConstData _playerConstData;
        public PlayerConstData PlayerConstData => _playerConstData;

        private static readonly int OnDamagedAnimKey = Animator.StringToHash("OnDamaged");

        public bool IsBattleStarted { get; private set; }


        public PlayerParameter PlayerParameter { get; } = new();

        private CinemachineImpulseSource _cinemachineImpulseSource;

        private Vector3 _animatorLocalPosition;

        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            _cinemachineImpulseSource = FindObjectOfType<CinemachineImpulseSource>();
            PlayerInput = GetComponent<PlayerInput>();

            _animatorLocalPosition = Animator.transform.localPosition;

            PlayerParameter.LifeObservable.Where(value => value <= 0).Subscribe(value =>
            {
                gameLoop.SendEvent(GameLoop.GameEvent.Lose);
                OnDead();
            }).AddTo(this);

            gameLoop.Event.Where(value => value == GameLoop.GameEvent.Win)
                .Subscribe(_ => PlayerParameter.Invincible = true).AddTo(this);


            gameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => IsBattleStarted = true);
        }


        public override void OnAttacked(AttackHitController attackHitController)
        {
            if (PlayerParameter.Invincible)
                return;


            var attackData = attackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
                PlayerParameter.Life -= attackData.Damage;

            _cinemachineImpulseSource.GenerateImpulse();
            attackHitEffectFactory.Create(transform.position, transform.rotation);

            Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                .OnComplete(() => Animator.transform.localPosition = _animatorLocalPosition);
            Animator.SetTrigger(OnDamagedAnimKey);
        }


        private void OnDead()
        {
            var targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            var deadObj = new GameObject("DeadObject")
            {
                transform =
                {
                    position = transform.position
                }
            };

            targetGroup.AddMember(deadObj.transform, 1, 0);


            gameObject.SetActive(false);
        }

        public override OwnerType GetOwnerType()
        {
            return OwnerType.Player;
        }

        public Vector2 GetDirectionToPlayer(Vector2 from)
        {
            return ((Vector2)transform.position - from).normalized;
        }
    }
}