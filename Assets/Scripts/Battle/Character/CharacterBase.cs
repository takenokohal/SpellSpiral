using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
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
        
        public CharacterData CharacterData { get; private set; }

        [Inject] protected AttackHitEffectFactory AttackHitEffectFactory { get; private set; }
        [Inject] protected AttackDatabase AttackDatabase { get; private set; }
        [Inject] protected CharacterDatabase CharacterDatabase { get; private set; }
        [Inject] protected AllCharacterManager AllCharacterManager { get; private set; }
        [Inject] protected GameLoop GameLoop { get; private set; }
        [Inject] protected MagicCircleFactory MagicCircleFactory { get; private set; }

        [Inject] protected ServantFactory ServantFactory { get; private set; }
        [Inject] protected CharacterCamera CharacterCamera { get; private set; }


        public virtual void AcquiredInject(
            AttackHitEffectFactory attackHitEffectFactory,
            AttackDatabase attackDatabase,
            CharacterDatabase characterDatabase,
            AllCharacterManager allCharacterManager,
            GameLoop gameLoop,
            MagicCircleFactory magicCircleFactory,
            ServantFactory servantFactory,
            CharacterCamera characterCamera)
        {
            AttackHitEffectFactory = attackHitEffectFactory;
            AttackDatabase = attackDatabase;
            CharacterDatabase = characterDatabase;
            AllCharacterManager = allCharacterManager;
            GameLoop = gameLoop;
            MagicCircleFactory = magicCircleFactory;
            ServantFactory = servantFactory;
            CharacterCamera = characterCamera;
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
            CharacterRotation = new CharacterRotation(transform);

            CharacterData = CharacterDatabase.Find(characterKey);

            AllCharacterManager.RegisterCharacter(this);

            InitializeFunction();

            IsInitialized = true;
        }

        public abstract void OnAttacked(AttackHitController attackHitController);

        //abstractにするかも
        public bool CheckHit(AttackHitController attackHitController)
        {
            var key = attackHitController.AttackKey;
            var data = AttackDatabase.Find(key);
            var attackOwner = data.OwnerType;
            return GetOwnerType() != attackOwner;
        }

        public OwnerType GetOwnerType()
        {
            return CharacterData.OwnerType;
        }
    }
}