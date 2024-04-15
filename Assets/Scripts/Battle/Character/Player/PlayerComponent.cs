using Databases;
using Sirenix.OdinInspector;

namespace Battle.Character.Player
{
    public abstract class PlayerComponent : SerializedMonoBehaviour
    {
        protected PlayerCore PlayerCore { get; private set; }
        protected PlayerConstData PlayerConstData => PlayerCore.PlayerConstData;

        protected PlayerParameter PlayerParameter => PlayerCore.PlayerParameter;

        protected bool IsPlayerDead => PlayerCore.IsDead;
        
        protected bool IsInitialized { get; private set; }

        protected bool IsBattleStarted => PlayerCore.IsBattleStarted;

        private async void Start()
        {
            PlayerCore = GetComponent<PlayerCore>();
            await PlayerCore.WaitUntilInitialize();
            Init();

            IsInitialized = true;
        }

        protected abstract void Init();
    }
}