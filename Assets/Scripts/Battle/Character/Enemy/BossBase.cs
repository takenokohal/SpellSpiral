using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battle.MyCamera;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Others;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace Battle.Character.Enemy
{
    public abstract class BossBase<T> : EnemyBase where T : Enum
    {
        [SerializeField] private List<BossSequenceBase<T>> sequencePrefabs;
        private readonly Dictionary<T, BossSequenceBase<T>> _sequenceInstances = new();

        protected T CurrentState { get; private set; }

        protected readonly CancellationTokenSource commonCancellationTokenSource = new();

        private Vector2 _animationBlendValue;
        public Vector2 ToAnimationVelocity { get; set; }
        private readonly int _horizontalAnimKey = Animator.StringToHash("HorizontalSpeed");
        private readonly int _verticalAnimKey = Animator.StringToHash("VerticalSpeed");


        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            var tg = FindObjectOfType<CinemachineTargetGroup>();
            tg.AddMember(transform, 1, 0);
            AllCharacterManager.RegisterBoss(this);

            Observable.EveryFixedUpdate().Subscribe(_ => MyFixedUpdate()).AddTo(this);

            CreateSequence();
            GameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => { BattleStart().Forget(); })
                .AddTo(this);
        }

        protected UniTask PlayState(T nextState)
        {
            CurrentState = nextState;
            return _sequenceInstances[CurrentState].SequenceStart();
        }


        private async UniTaskVoid BattleStart()
        {
            await WaitUntilInitialize();


            gameObject.OnDestroyAsObservable().Subscribe(_ => commonCancellationTokenSource.Cancel());


            OnDeadObservable().Take(1).Subscribe(_ =>
            {
                commonCancellationTokenSource.Cancel();
                GameLoop.SendEvent(GameLoop.GameEvent.Win);
            });
        }

        private void CreateSequence()
        {
            foreach (var prefab in sequencePrefabs)
            {
                _sequenceInstances.Add(prefab.StateKey, prefab.Construct(
                    new BossSequenceBase<T>.SequenceRequiredComponents()
                    {
                        AllCharacterManager = AllCharacterManager,
                        Parent = this,
                        PlayerCore = playerCore,
                        SpecialCameraSwitcher = specialCameraSwitcher,
                        MagicCircleFactory = MagicCircleFactory,
                        ServantFactory = ServantFactory
                    }));
            }
        }

        private void MyFixedUpdate()
        {
            if (GameLoop.CurrentState != GameLoop.GameEvent.BattleStart)
                return;


            var to = ToAnimationVelocity.normalized;
            to.x *= CharacterRotation.Rotation;
            _animationBlendValue = Vector2.Lerp(_animationBlendValue, to, 0.2f);

            Animator.SetFloat(_horizontalAnimKey, _animationBlendValue.x);
            Animator.SetFloat(_verticalAnimKey, _animationBlendValue.y);
        }
    }
}