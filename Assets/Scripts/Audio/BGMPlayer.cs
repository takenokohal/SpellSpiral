using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Audio
{
    public class BGMPlayer : IInitializable
    {
        public void Initialize()
        {
            /*
            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => AudioManager.IsInitialized);
                AudioManager.PlaySe("Tempest");
            });
            */
        }
    }
}