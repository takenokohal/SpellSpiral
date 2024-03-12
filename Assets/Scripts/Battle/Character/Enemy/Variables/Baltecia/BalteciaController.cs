using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battle.Character.Player;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
using Cysharp.Threading.Tasks;
using Others;
using Others.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UniRx;
using UniRx.Triggers;
using VContainer;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class BalteciaController : SerializedMonoBehaviour
    {
        [Inject] private readonly PlayerCore _playerCore;
        [Inject] private readonly AllEnemyManager _allEnemyManager;
        [Inject] private readonly SpecialCameraSwitcher _specialCameraSwitcher;
        [Inject] private readonly MagicCircleFactory _magicCircleFactory;
        [Inject] private readonly GameLoop _gameLoop;
        private EnemyCore _enemyCore;


        [OdinSerialize] private readonly Dictionary<State, EnemySequenceBase> _sequencePrefabs = new();
        private readonly Dictionary<State, EnemySequenceBase> _sequenceInstances = new();

        private State _currentState;

        private readonly CancellationTokenSource _cancellationTokenSource = new();
        
        private void Start()
        {
            _enemyCore = GetComponent<EnemyCore>();

            _gameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => { Init().Forget(); })
                .AddTo(this);
        }

        private async UniTaskVoid Init()
        {
            await UniTask.WaitWhile(() => !_enemyCore.IsInitialized);

            foreach (var prefab in _sequencePrefabs)
            {
                _sequenceInstances.Add(prefab.Key, prefab.Value.Construct(
                    new EnemySequenceBase.SequenceRequiredComponents()
                    {
                        AllEnemyManager = _allEnemyManager,
                        Parent = _enemyCore,
                        PlayerCore = _playerCore,
                        SpecialCameraSwitcher = _specialCameraSwitcher,
                        MagicCircleFactory = _magicCircleFactory
                    }));
            }

            gameObject.OnDestroyAsObservable().Subscribe(_ => _cancellationTokenSource.Cancel());

            _enemyCore.EnemyParameter.CurrentLifeObservable
                .Where(value => value <= 0)
                .Take(1)
                .Subscribe(_ =>
                {
                    _cancellationTokenSource.Cancel();
                    _gameLoop.SendEvent(GameLoop.GameEvent.Win);
                });


            Loop().Forget();
        }


        private async UniTask Loop()
        {
            _currentState = State.MissileCarnival;

            await _sequenceInstances[_currentState].SequenceStart();

            var states = new List<State>
            {
                State.TackleAndShotgun,
                State.BackStepAndGatlingAndLaser,
                State.CornerGatling3,
                State.FlameSword,
                State.Explosion,
                State.ShortHomingBullet
            };

            while (!_cancellationTokenSource.IsCancellationRequested)
            {
                var nextState = states.Where(value => value != _currentState).GetRandomValue();


                _currentState = nextState;

                _enemyCore.LookPlayer();
                await _sequenceInstances[_currentState].SequenceStart();
            }
        }

        public enum State
        {
            TackleAndShotgun,
            BackStepAndGatlingAndLaser,
            CornerGatling3,

            FlameSword,
            Explosion,
            ShortHomingBullet,

            //Special
            MissileCarnival,
            
            SummonServant
        }
    }
}