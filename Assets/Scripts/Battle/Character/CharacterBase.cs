using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Databases;
using Others;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Battle.Character
{
    public abstract class CharacterBase : SerializedMonoBehaviour, IAttackHittable
    {
        [SerializeField] private string characterKey;
        public string CharacterKey => characterKey;
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public CharacterRotation CharacterRotation { get; private set; }

        //Centerも消す。Modelを中央に。
        [Inject] protected readonly AttackHitEffectFactory attackHitEffectFactory;
        [Inject] protected readonly AttackDatabase attackDatabase;
        [Inject] protected readonly CharacterDatabase characterDatabase;
        [Inject] protected readonly AllEnemyManager allEnemyManager;
        [Inject] protected readonly GameLoop gameLoop;
        [Inject] protected readonly PlayerCore playerCore;
        [Inject] protected readonly MagicCircleFactory magicCircleFactory;


        public bool IsInitialized { get; protected set; }
        public UniTask WaitUntilInitialize() => UniTask.WaitUntil(() => IsInitialized);

        protected virtual void InitializeFunction()
        {
        }

        private void Start()
        {
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            CharacterRotation = new CharacterRotation(transform);

            InitializeFunction();

            IsInitialized = true;
        }

        public abstract void OnAttacked(AttackHitController attackHitController);

        public abstract OwnerType GetOwnerType();
    }
}