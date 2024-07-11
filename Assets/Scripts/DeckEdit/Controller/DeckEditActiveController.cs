using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DeckEdit.Model;
using DG.Tweening;
using Others.Dialog;
using Others.Input;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace DeckEdit.Controller
{
    public class DeckEditActiveController : MonoBehaviour
    {
        [Inject] private readonly MyInputManager _myInputManager;

        [Inject] private readonly MyDeckModel _myDeckModel;

        [Inject] private readonly YesNoDialog _yesNoDialog;
        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;

        [SerializeField] private CanvasGroup canvasGroup;


        public static bool IsActive { get; private set; }

        public static async UniTask StartDeckEdit()
        {
            IsActive = true;
            await SceneManager.LoadSceneAsync("DeckEdit", LoadSceneMode.Additive);
        }

        private void Start()
        {
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.5f);
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (_myInputManager.PlayerInput.actions["No"].WasPressedThisFrame())
                TryFinish().Forget();
        }

        private async UniTaskVoid TryFinish()
        {
            var isFilled = _myDeckModel.IsFilled;
            if (!isFilled)
            {
                var result = await _yesNoDialog.Open("デッキ枚数が足りません。変更を破棄して終了しますか？");
                switch (result)
                {
                    case YesNoDialog.YesNo.Yes:
                        Close().Forget();
                        break;
                    case YesNoDialog.YesNo.No:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                _deckSaveDataPresenter.SaveDeck(new DeckData(
                    _myDeckModel.CurrentDeckList.Select(value => value.Key).ToList(),
                    _myDeckModel.CurrentHighlanderSpell.Key));
                Close().Forget();
            }
        }

        private async UniTaskVoid Close()
        {
            IsActive = false;

            await canvasGroup.DOFade(0, 0.2f);
            await SceneManager.UnloadSceneAsync("DeckEdit");
        }
    }
}