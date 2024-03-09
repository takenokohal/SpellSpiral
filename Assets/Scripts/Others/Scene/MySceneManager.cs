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

        public static string CurrentSceneName => SceneManager.GetActiveScene().name;

        public async UniTask ChangeSceneAsync(string nextScene)
        {
            if (Changing)
                return;

            PrevSceneName = CurrentSceneName;

            Changing = true;
            var asyncOperation = SceneManager.LoadSceneAsync(nextScene);
            asyncOperation.allowSceneActivation = false;

            await _sceneFadePanelView.FadeOut();

            await UniTask.Delay(500);

            asyncOperation.allowSceneActivation = true;

            await _sceneFadePanelView.FadeIn();

            Changing = false;
        }
    }
}