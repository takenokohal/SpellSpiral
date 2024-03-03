using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Others
{
    public class SceneChanger : MonoBehaviour
    {
        [SerializeField] private Image panel;

        private static SceneChanger _instance;

        public static bool Fading { get; private set; }

        private void Start()
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public static string CurrentSceneName => SceneManager.GetActiveScene().name;

        public static UniTask ChangeSceneAsync(string nextScene)
        {
            return _instance.ChangeScene(nextScene);
        }

        private async UniTask ChangeScene(string nextScene)
        {
            Fading = true;
            var asyncOperation = SceneManager.LoadSceneAsync(nextScene);
            asyncOperation.allowSceneActivation = false;

            await panel.DOFade(1, 1);

            await UniTask.Delay(1000);

            asyncOperation.allowSceneActivation = true;

            await panel.DOFade(0, 1);

            Fading = false;
        }
    }
}