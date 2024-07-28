using Battle.Character.Player.Buff;
using Cysharp.Threading.Tasks;
using Databases;
using Sirenix.OdinInspector;

namespace Battle.Character.Player
{
    public abstract class PlayerComponent : SerializedMonoBehaviour
    {
        protected PlayerCore PlayerCore { get; private set; }

        protected PlayerConstData PlayerConstData => PlayerCore.PlayerConstData;

        protected PlayerParameter PlayerParameter => PlayerCore.PlayerParameter;

        protected PlayerBuff PlayerBuff => PlayerCore.PlayerBuff;

        protected WizardAnimationController WizardAnimationController => PlayerCore.WizardAnimationController;

        protected bool IsPlayerDead => PlayerCore.IsDead;

        protected bool IsInitialized { get; private set; }

        protected bool IsBattleStarted => PlayerCore.IsBattleStarted;

        private void Start()
        {
            UniTask.Void(async () =>
            {
                PlayerCore = GetComponent<PlayerCore>();

                await PlayerCore.WaitUntilInitialize();
                Init();

                IsInitialized = true;
            });
        }

        protected abstract void Init();

        protected UniTask MyDelay(float duration)
        {
            return UniTask.Delay((int)(duration * 1000), cancellationToken: destroyCancellationToken);
        }
    }
}