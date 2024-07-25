using System.Linq;
using Battle.Character;
using Battle.System.Main;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using Others.Scene;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Battle.System.Result
{
    public class WinController : MonoBehaviour
    {
        [Inject] private readonly BattleLoop _battleLoop;
        [Inject] private readonly AllCharacterManager _allCharacterManager;
        [Inject] private readonly MyInputManager _myInputManager;

        [SerializeField] private ParticleSystem effect;

        [SerializeField] private CanvasGroup mainCanvas;

        [SerializeField] private Image whiteOut;
        [SerializeField] private TMP_Text missionCompleteText;

        [SerializeField] private GameObject winCamera;

        [Inject] private readonly MySceneManager _mySceneManager;

        private void Start()
        {
            _battleLoop.Event
                .Where(value => value == BattleEvent.Win)
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

            await UniTask.Delay(1000, cancellationToken: destroyCancellationToken);

            whiteOut.gameObject.SetActive(true);
            await whiteOut.DOFade(1, 1);
            Time.timeScale = 1;

            missionCompleteText.gameObject.SetActive(true);
            await missionCompleteText.transform.DOScaleY(0, 0);
            await missionCompleteText.transform.DOScaleY(1, 0.2f);


            _myInputManager.PlayerInput.SwitchCurrentActionMap("UI");
            await UniTask.WaitUntil(() =>
                _myInputManager.PlayerInput.actions.Any(value => value.WasPressedThisFrame()));

            await missionCompleteText.transform.DOScaleY(0, 0.2f);

            _mySceneManager.ChangeSceneAsync("Home").Forget();
        }
    }
}