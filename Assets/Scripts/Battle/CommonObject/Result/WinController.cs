using System.Linq;
using Battle.Character.Enemy;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using Others.Scene;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Battle.CommonObject.Result
{
    public class WinController : MonoBehaviour
    {
        [Inject] private readonly GameLoop _gameLoop;
        [Inject] private readonly AllEnemyManager _allEnemyManager;

        [SerializeField] private ParticleSystem effect;

        [SerializeField] private CanvasGroup mainCanvas;

        //  [SerializeField] private CanvasGroup loseCanvas;
        [SerializeField] private Image whiteOut;

        [SerializeField] private PlayerInput input;

        [SerializeField] private GameObject winCamera;

        [Inject] private readonly MySceneManager _mySceneManager;

        private void Start()
        {
            _gameLoop.Event
                .Where(value => value == GameLoop.GameEvent.Win)
                .Subscribe(_ => OnWin().Forget())
                .AddTo(this);
        }

        private async UniTaskVoid OnWin()
        {
            Time.timeScale = 0.5f;

            winCamera.SetActive(true);

            await UniTask.Delay(1000);


            var enemy = _allEnemyManager.Boss;
            effect.transform.position = enemy.transform.position;
            effect.Play();

            await mainCanvas.DOFade(0, 1);

            /*
            loseCanvas.gameObject.SetActive(true);

            loseCanvas.DOFade(0, 0);

            await loseCanvas.DOFade(1, 1f);
            

            var buttons = input.currentActionMap
                .Where(value => value.type == InputActionType.Button);

            await UniTask.WaitUntil(() => buttons.Any(value => value.WasPressedThisFrame()));
            
            */

            whiteOut.gameObject.SetActive(true);
            await whiteOut.DOFade(1, 3);

            Time.timeScale = 1;

            _mySceneManager.ChangeSceneAsync("Home").Forget();
        }
    }
}