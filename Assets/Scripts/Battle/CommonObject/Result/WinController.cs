using System.Linq;
using Battle.Character.Enemy;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using Others.Input;
using Others.Scene;
using TMPro;
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
        [Inject] private readonly AllCharacterManager _allCharacterManager;
        [Inject] private readonly MyInputManager _myInputManager;

        [SerializeField] private ParticleSystem effect;

        [SerializeField] private CanvasGroup mainCanvas;

        //  [SerializeField] private CanvasGroup loseCanvas;
        [SerializeField] private Image whiteOut;
        [SerializeField] private TMP_Text missionCompleteText;

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

            await UniTask.Delay(500);


            var enemy = _allCharacterManager.Boss;
            effect.transform.position = enemy.transform.position;
            effect.Play();
            enemy.Animator.gameObject.SetActive(false);

            //  await mainCanvas.DOFade(0, 1);

            /*
            loseCanvas.gameObject.SetActive(true);

            loseCanvas.DOFade(0, 0);

            await loseCanvas.DOFade(1, 1f);
            

            var buttons = input.currentActionMap
                .Where(value => value.type == InputActionType.Button);

            await UniTask.WaitUntil(() => buttons.Any(value => value.WasPressedThisFrame()));
            
            */

            await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);

            whiteOut.gameObject.SetActive(true);
            await whiteOut.DOFade(1, 1);
            Time.timeScale = 1;

            missionCompleteText.gameObject.SetActive(true);
            await missionCompleteText.transform.DOScaleY(0, 0);
            await missionCompleteText.transform.DOScaleY(1, 0.2f);

            await UniTask.WaitUntil(() => _myInputManager.UiInput.actions.Any(value => value.WasPressedThisFrame()));

            await missionCompleteText.transform.DOScaleY(0, 0.2f);

            _mySceneManager.ChangeSceneAsync("Home").Forget();
        }
    }
}