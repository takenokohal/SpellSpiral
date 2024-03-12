using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DeckEdit.View
{
    public class ActiveAreaController : MonoBehaviour
    {
        [Inject] private readonly DeckCursorView _deckCursorView;
        [Inject] private readonly CardPoolCursorView _cardPoolCursorView;
        [Inject] private readonly PlayerInput _playerInput;


        private void Start()
        {
            SetArea(false);
        }

        private void Update()
        {
            if (_playerInput.actions["R"].WasPressedThisFrame())
                SetArea(false);

            if (_playerInput.actions["L"].WasPressedThisFrame())
                SetArea(true);
        }

        private void SetArea(bool isDeck)
        {
            _deckCursorView.SetActive(isDeck);
            _cardPoolCursorView.SetActive(!isDeck);
        }
    }
}