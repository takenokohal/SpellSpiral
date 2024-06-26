using System;
using Cysharp.Threading.Tasks;
using Others;
using Others.Dialog;
using Others.Input;
using Others.Scene;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Model
{
    public class ReturnAndSave : ITickable
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;
        [Inject] private readonly YesNoDialog _yesNoDialog;
        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly MySceneManager _mySceneManager;
        [Inject] private readonly DeckList _deckList;

        public void Tick()
        {
            if (_yesNoDialog.IsOpen)
                return;

            if (_okDialog.IsOpen)
                return;
            
            if (!PlayerInput.actions["No"].WasPressedThisFrame())
                return;


            if (_deckList.IsFilled)
                DialogCheck().Forget();
            else
            {
                _okDialog.Open("デッキ枚数を20枚にしてください").Forget();
            }
        }

        private async UniTaskVoid DialogCheck()
        {
            var result = await _yesNoDialog.Open("デッキ編集を終了します");

            switch (result)
            {
                case YesNoDialog.YesNo.Yes:
                    var nextScene = _mySceneManager.PrevSceneName ?? "Home";
                    _mySceneManager.ChangeSceneAsync(nextScene).Forget();
                    _deckList.Save();

                    break;
                case YesNoDialog.YesNo.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}