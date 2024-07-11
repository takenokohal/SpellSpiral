/*using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DeckEdit.Model;
using Others.Dialog;
using Others.Input;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Controller
{
    public class DeckEditFinishController : ITickable
    {
        [Inject] private readonly MyDeckModel _myDeckModel;
        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;
        [Inject] private readonly MyInputManager _myInputManager;

        [Inject] private readonly YesNoDialog _yesNoDialog;
        [Inject] private readonly DeckEditActiveController _deckEditActiveController;

        public void Tick()
        {
            if (!_deckEditActiveController.IsActive)
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
                        Close();
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
                Close();
            }

            #1#
        }

        private void Close()
        {
            IsActive = false;
            canvasGroup.DOFade(0, 0.2f).OnComplete(() => { gameObject.SetActive(false); });
        }
    }
}*/