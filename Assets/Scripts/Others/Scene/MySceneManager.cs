using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using VContainer;

namespace Others.Scene
{
    public class MySceneManager
    {
        [Inject] private readonly SceneFadePanelView _sceneFadePanelView;

        public bool Changing { get; private set; }

        public string PrevSceneName { get; private set; }


        public string CurrentSceneName { get; private set; }

        public string CurrentAdditiveSceneName { get; private set; }

        public MySceneManager()
        {
            CurrentSceneName = SceneManager.GetActiveScene().name;
        }

        public async UniTask ChangeSceneAsync(string nextScene)
        {
            if (Changing)
                return;
            
            PrevSceneName = CurrentSceneName ?? "Home";
            CurrentSceneName = nextScene;
            CurrentAdditiveSceneName = null;

            Changing = true;

            var asyncOperation = SceneManager.LoadSceneAsync(nextScene);
            asyncOperation.allowSceneActivation = false;
            await _sceneFadePanelView.FadeOut();

            asyncOperation.allowSceneActivation = true;
            await UniTask.Delay(500, DelayType.UnscaledDeltaTime);

            await asyncOperation;


            await _sceneFadePanelView.FadeIn();

            Changing = false;
        }

        public async UniTask LoadSceneAdditive(string sceneName)
        {
            CurrentAdditiveSceneName = sceneName;
            await SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}