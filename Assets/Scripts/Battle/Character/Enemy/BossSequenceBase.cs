using System;
using System.Threading;
using Audio;
using Battle.Character.Player;
using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
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
    public abstract class BossSequenceBase<T> : SerializedMonoBehaviour where T : Enum
    {
        public class SequenceRequiredComponents
        {
            public BossControllerBase<T> Parent { get; set; }

            public PlayerCore PlayerCore { get; set; }
            public AllCharacterManager AllCharacterManager { get; set; }

            public CameraSwitcher CameraSwitcher { get; set; }

            public MagicCircleFactory MagicCircleFactory { get; set; }
            public ReadyEffectFactory ReadyEffectFactory { get; set; }

            public CharacterFactory CharacterFactory { get; set; }
        }


        protected BossControllerBase<T> Parent { get; private set; }

        protected string CharacterKey => Parent.CharacterKey;
        protected PlayerCore PlayerCore { get; private set; }

        protected AllCharacterManager AllCharacterManager { get; private set; }

        protected CameraSwitcher CameraSwitcher { get; private set; }

        protected MagicCircleFactory MagicCircleFactory { get; private set; }

        protected ReadyEffectFactory ReadyEffectFactory { get; private set; }

        protected CharacterFactory CharacterFactory { get; private set; }

        public CancellationTokenSource SequenceCancellationToken { get; private set; }

        public WizardAnimationController WizardAnimationController => Parent.WizardAnimationController;


        public abstract T StateKey { get; }

        public void CancelSequence()
        {
            SequenceCancellationToken.Cancel();

            SequenceCancellationToken = new CancellationTokenSource();
        }


        public BossSequenceBase<T> Construct(SequenceRequiredComponents sequenceRequiredComponents)
        {
            var instance = Instantiate(this);

            instance.Init(sequenceRequiredComponents).Forget();

            return instance;
        }

        private async UniTaskVoid Init(SequenceRequiredComponents sequenceRequiredComponents)
        {
            Parent = sequenceRequiredComponents.Parent;
            PlayerCore = sequenceRequiredComponents.PlayerCore;
            AllCharacterManager = sequenceRequiredComponents.AllCharacterManager;
            CameraSwitcher = sequenceRequiredComponents.CameraSwitcher;

            MagicCircleFactory = sequenceRequiredComponents.MagicCircleFactory;
            ReadyEffectFactory = sequenceRequiredComponents.ReadyEffectFactory;
            CharacterFactory = sequenceRequiredComponents.CharacterFactory;

            SequenceCancellationToken = new CancellationTokenSource();

            gameObject.OnDestroyAsObservable().Subscribe(_ => SequenceCancellationToken?.Cancel());

            gameObject.SetActive(true);

            await UniTask.WaitWhile(() => !Parent.IsInitialized);

            Parent.OnDeadObservable()
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
            tween.SetUpdate(UpdateType.Fixed);
            return tween.ToUniTask(cancellationToken: SequenceCancellationToken.Token);
        }
    }
}