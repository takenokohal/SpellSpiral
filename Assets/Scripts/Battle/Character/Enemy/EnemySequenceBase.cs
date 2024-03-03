using System.Threading;
using Battle.Character.Player;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.Character.Enemy
{
    //基本Destroyせずに使いまわす。
    public abstract class EnemySequenceBase : SerializedMonoBehaviour
    {
        public class SequenceRequiredComponents
        {
            public EnemyCore Parent { get; set; }
            public PlayerCore PlayerCore { get; set; }
            public AllEnemyManager AllEnemyManager { get; set; }

            public SpecialCameraSwitcher SpecialCameraSwitcher { get; set; }

            public MagicCircleFactory MagicCircleFactory { get; set; }
        }


        protected EnemyCore Parent { get; private set; }
        protected PlayerCore PlayerCore { get; private set; }

        protected AllEnemyManager AllEnemyManager { get; private set; }

        protected SpecialCameraSwitcher SpecialCameraSwitcher { get; private set; }

        protected MagicCircleFactory MagicCircleFactory { get; private set; }

        public CancellationTokenSource SequenceCancellationToken { get; private set; }

        public Animator Animator => Parent.Animator;


        public void CancelSequence()
        {
            SequenceCancellationToken.Cancel();

            SequenceCancellationToken = new CancellationTokenSource();
        }


        public EnemySequenceBase Construct(SequenceRequiredComponents sequenceRequiredComponents)
        {
            var instance = Instantiate(this);

            instance.Init(sequenceRequiredComponents).Forget();

            return instance;
        }

        private async UniTaskVoid Init(SequenceRequiredComponents sequenceRequiredComponents)
        {
            Parent = sequenceRequiredComponents.Parent;
            PlayerCore = sequenceRequiredComponents.PlayerCore;
            AllEnemyManager = sequenceRequiredComponents.AllEnemyManager;
            SpecialCameraSwitcher = sequenceRequiredComponents.SpecialCameraSwitcher;

            MagicCircleFactory = sequenceRequiredComponents.MagicCircleFactory;

            SequenceCancellationToken = new CancellationTokenSource();

            gameObject.OnDestroyAsObservable().Subscribe(_ => SequenceCancellationToken?.Cancel());

            gameObject.SetActive(true);

            await UniTask.WaitWhile(() => !Parent.IsInitialized);

            Parent.EnemyParameter.CurrentLifeObservable
                .Where(value => value <= 0)
                .Take(1).Subscribe(_ => CancelSequence());
        }

        public UniTask SequenceStart()
        {
            SequenceCancellationToken = new CancellationTokenSource();
            return Sequence();
        }

        protected abstract UniTask Sequence();

        protected Vector2 GetDirectionToPlayer() => Parent.GetDirectionToPlayer();

        protected UniTask MyDelay(float time)
        {
            return UniTask.Delay(
                (int)(time * 1000f), cancellationToken: SequenceCancellationToken.Token);
        }

        protected UniTask TweenToUniTask(Tweener tween)
        {
            return tween.ToUniTask(cancellationToken: SequenceCancellationToken.Token);
        }
    }
}