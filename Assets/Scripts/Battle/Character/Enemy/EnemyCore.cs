using Battle.Attack;
using Battle.Character.Player;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Others;
using UnityEngine;
using VContainer;

namespace Battle.Character.Enemy
{
    public class EnemyCore : MonoBehaviour, IAttackHittable
    {
        [Inject] private readonly AllEnemyManager _allEnemyManager;
        [Inject] private readonly PlayerCore _playerCore;
        private CinemachineImpulseSource _impulseSource;
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly AttackHitEffectFactory _attackHitEffectFactory;
        [Inject] private readonly AttackDatabase _attackDatabase;
        [Inject] private readonly GameLoop _gameLoop;

        [SerializeField] private bool isBoss;
        public bool IsBoss => isBoss;

        [SerializeField] private CharacterKey characterKey;


        [SerializeField] private Transform center;
        public Transform Center => center;

        public Rigidbody Rigidbody { get; private set; }

        public CharacterRotation CharacterRotation { get; private set; }

        public Animator Animator { get; private set; }
        public bool IsInitialized { get; private set; }

        private Vector2 _animationBlendValue;
        public Vector2 ToAnimationVelocity { get; set; }
        public static int HorizontalAnimKey { get; } = Animator.StringToHash("HorizontalSpeed");
        public static int VerticalAnimKey { get; } = Animator.StringToHash("VerticalSpeed");


        public Vector2 GetDirectionToPlayer()
        {
            return (_playerCore.Center.position - Center.position).normalized;
        }


        public EnemyParameter EnemyParameter { get; private set; }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            CharacterRotation = GetComponent<CharacterRotation>();
            Animator = GetComponentInChildren<Animator>();
            _allEnemyManager.RegisterEnemy(this);

            _impulseSource = FindObjectOfType<CinemachineImpulseSource>();

            EnemyParameter = new EnemyParameter(_characterDatabase.Find(characterKey).Life);
            IsInitialized = true;
        }

        private void OnDestroy()
        {
            _allEnemyManager.RemoveEnemy(this);
        }

        public void OnAttacked(AttackHitController attackHitController)
        {
            var attackData = _attackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
                EnemyParameter.CurrentLife -= attackData.Damage;
            _impulseSource.GenerateImpulse(5);
            
            
            Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                .OnComplete(() => Animator.transform.localPosition = Vector3.zero);
            _attackHitEffectFactory.Create(Center.position, Center.rotation);
        }

        public OwnerType GetOwnerType()
        {
            return OwnerType.Enemy;
        }

        public void LookPlayer()
        {
            CharacterRotation.Rotation = GetDirectionToPlayer().x;
        }

        public async UniTask DeadAnimation()
        {
            await UniTask.Delay(1000);
        }

        private void FixedUpdate()
        {
            if(_gameLoop.CurrentState != GameLoop.GameEvent.BattleStart)
                return;
            var to = ToAnimationVelocity.normalized;
            to.x *= CharacterRotation.Rotation;
            _animationBlendValue = Vector2.Lerp(_animationBlendValue, to, 0.2f);

            Animator.SetFloat(HorizontalAnimKey, _animationBlendValue.x);
            Animator.SetFloat(VerticalAnimKey, _animationBlendValue.y);
        }
    }
}