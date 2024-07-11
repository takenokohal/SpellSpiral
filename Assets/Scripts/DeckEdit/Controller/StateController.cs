using Audio;
using DeckEdit.Model;
using DeckEdit.View;
using DeckEdit.View.CardPool;
using DeckEdit.View.MyDeck;
using Others;
using Others.Dialog;
using Others.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Controller
{
    public class StateController : IInitializable
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;

        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;

        [Inject] private readonly DeckEditStateModel _stateModel;

        [Inject] private readonly MyDeckCursorView _myDeckCursorView;
        [Inject] private readonly CardPoolCursorView _cardPoolCursorView;

        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;


        private void SetArea(bool isDeck)
        {
          //  AllAudioManager.PlaySe("Change");

         //  _stateModel.CurrentState = isDeck ? DeckEditState.MyDeck : DeckEditState.CardPool;
        }

        void IInitializable.Initialize()
        {
      //      SetArea(true);
        }
    }
}