using Audio;
using Others;
using Others.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DeckEdit.View
{
    public class ActiveAreaController : MonoBehaviour
    {
        [Inject] private readonly DeckCursorView _deckCursorView;
        [Inject] private readonly CardPoolCursorView _cardPoolCursorView;
        [Inject] private readonly MyInputManager _myInputManager;

        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;
        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;

        private void Start()
        {
            SetArea(false);
        }

        private void Update()
        {
            if(AnyDialogIsOpen)
                return;
            
            if (_myInputManager.UiInput.actions["R"].WasPressedThisFrame())
                SetArea(false);

            if (_myInputManager.UiInput.actions["L"].WasPressedThisFrame())
                SetArea(true);
        }

        private void SetArea(bool isDeck)
        {
            AllAudioManager.PlaySe("Change");
            _deckCursorView.SetActive(isDeck);
            _cardPoolCursorView.SetActive(!isDeck);
        }
    }
}