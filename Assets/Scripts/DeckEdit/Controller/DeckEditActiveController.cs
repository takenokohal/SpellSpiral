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

        [Inject] private readonly ChoiceDialog _choiceDialog;
        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;

        [SerializeField] private CanvasGroup canvasGroup;

        private static UniTaskCompletionSource _uniTaskCompletionSource;

        public static bool IsActive { get; private set; }

        public static async UniTask StartDeckEdit()
        {
            await SceneManager.LoadSceneAsync("DeckEdit", LoadSceneMode.Additive);
            _uniTaskCompletionSource = new UniTaskCompletionSource();
            await _uniTaskCompletionSource.Task;
        }

        private void Start()
        {
            IsActive = true;

            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, 0.5f);
        }

        private void Update()
        {
            if (!IsActive)
            {
                return;
            }

            if (_choiceDialog.IsOpen)
                return;

            if (_myInputManager.PlayerInput.actions["No"].WasPressedThisFrame())
                TryFinish();
        }

        private void TryFinish()
        {
            var isFilled = _myDeckModel.IsFilled;
            //デッキ枚数足りない
            if (!isFilled)
            {
                FinishDialogNoFilled().Forget();
            }
            else
            {
                FinishDialogFilled().Forget();
            }
        }

        private async UniTask FinishDialogNoFilled()
        {
            var result = await _choiceDialog.StartDialog(
                new ChoiceDialogData("deck_edit_return_no_filled", new[]
                {
                    "deck_edit_return_discard",
                    "deck_edit_return_cancel",
                }));
            switch (result)
            {
                case 0:
                    Close().Forget();
                    break;
                case 1:
                case -1:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTask FinishDialogFilled()
        {
            var result = await _choiceDialog.StartDialog(
                new ChoiceDialogData("deck_edit_return", new[]
                {
                    "deck_edit_return_save",
                    "deck_edit_return_discard",
                    "deck_edit_return_cancel",
                }));
            switch (result)
            {
                case 0:
                    _deckSaveDataPresenter.SaveDeck(new DeckData(
                        _myDeckModel.CurrentDeckList.Select(value => value.Key).ToList(),
                        _myDeckModel.CurrentHighlanderSpell.Key));
                    Close().Forget();
                    break;
                case 1:
                    Close().Forget();
                    break;
                case 2:
                case -1:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private async UniTaskVoid Close()
        {
            IsActive = false;

            await canvasGroup.DOFade(0, 0.2f);
            await SceneManager.UnloadSceneAsync("DeckEdit");

            _uniTaskCompletionSource.TrySetResult();
        }
    }
}