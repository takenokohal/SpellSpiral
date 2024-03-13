using System;
using System.Collections.Generic;
using System.Threading;
using Battle.MyCamera;
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
        [Inject] private readonly SpecialCameraSwitcher _specialCameraSwitcher;
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

            allEnemyManager.RegisterBoss(this);
            allEnemyManager.RegisterEnemy(this);

            Observable.EveryFixedUpdate().Subscribe(_ => MyFixedUpdate());

            CreateSequence();
            gameLoop.Event
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

            EnemyParameter.CurrentLifeObservable
                .Where(value => value <= 0)
                .Take(1)
                .Subscribe(_ =>
                {
                    commonCancellationTokenSource.Cancel();
                    gameLoop.SendEvent(GameLoop.GameEvent.Win);
                });
        }

        private void CreateSequence()
        {
            foreach (var prefab in sequencePrefabs)
            {
                _sequenceInstances.Add(prefab.StateKey, prefab.Construct(
                    new BossSequenceBase<T>.SequenceRequiredComponents()
                    {
                        AllEnemyManager = allEnemyManager,
                        Parent = this,
                        PlayerCore = playerCore,
                        SpecialCameraSwitcher = _specialCameraSwitcher,
                        MagicCircleFactory = magicCircleFactory,
                    }));
            }
        }

        private void MyFixedUpdate()
        {
            if (gameLoop.CurrentState != GameLoop.GameEvent.BattleStart)
                return;


            var to = ToAnimationVelocity.normalized;
            to.x *= CharacterRotation.Rotation;
            _animationBlendValue = Vector2.Lerp(_animationBlendValue, to, 0.2f);

            Animator.SetFloat(_horizontalAnimKey, _animationBlendValue.x);
            Animator.SetFloat(_verticalAnimKey, _animationBlendValue.y);
        }
    }
}