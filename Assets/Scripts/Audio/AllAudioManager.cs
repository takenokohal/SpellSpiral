using VContainer;
using VContainer.Unity;

namespace Audio
{
    public class AllAudioManager : IInitializable
    {
        private static AllAudioManager _instance;

        [Inject] private readonly SeManager _seManager;


        public static SeSource PlaySe(string seName)
        {
            return _instance._seManager.PlaySe(seName);
        }

        void IInitializable.Initialize()
        {
            _instance = this;
        }
    }
}