using System.Linq;
using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace Battle.CommonObject.Result
{
    public class LoseController : MonoBehaviour
    {
        [Inject] private readonly GameLoop _gameLoop;
        [Inject] private readonly PlayerCore _playerCore;

        [SerializeField] private ParticleSystem effect;

        [SerializeField] private CanvasGroup mainCanvas;
        [SerializeField] private CanvasGroup loseCanvas;
        [SerializeField] private Image whiteOut;

        [SerializeField] private PlayerInput input;

        [SerializeField] private LoseMenu loseMenu;


        private void Start()
        {
            _gameLoop.Event
                .Where(value => value == GameLoop.GameEvent.Lose)
                .Subscribe(_ => OnLose().Forget())
                .AddTo(this);
        }

        private async UniTaskVoid OnLose()
        {
            effect.transform.position = _playerCore.transform.position;
            effect.Play();

            await mainCanvas.DOFade(0, 1);

            loseCanvas.gameObject.SetActive(true);

            loseCanvas.DOFade(0, 0);

            await loseCanvas.DOFade(1, 1f);

            loseMenu.Activate().Forget();
        }
    }
}