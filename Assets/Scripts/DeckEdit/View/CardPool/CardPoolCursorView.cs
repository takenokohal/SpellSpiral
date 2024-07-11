using System;
using Audio;
using DeckEdit.Model;
using Others;
using Others.Dialog;
using Others.Input;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DeckEdit.View.CardPool
{
    public class CardPoolCursorView : MonoBehaviour
    {
        [Inject] private readonly MyInputManager _myInputManager;

        [Inject] private readonly CardPoolModel _cardPoolModel;
        [Inject] private readonly CardPoolListView _cardPoolListView;
        [Inject] private readonly CardPoolScrollView _scrollView;

        [SerializeField] private int xLength;

        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;
        [Inject] private readonly DeckEditStateModel _deckEditStateModel;
        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;
        public Vector2Int CurrentPos { get; private set; }

        public int CurrentIndex
        {
            get => PosToIndex(CurrentPos);
            private set => CurrentPos = new Vector2Int(value % xLength, value / xLength);
        }

        private int PosToIndex(Vector2Int pos)
        {
            return pos.x + pos.y * xLength;
        }

        [SerializeField] private Transform cursor;

        private bool _isActive;

        private readonly Subject<Unit> _onMove = new();
        public IObservable<Unit> OnMove => _onMove.TakeUntilDestroy(this);

        private void Update()
        {
            if (!_isActive)
                return;

            if (AnyDialogIsOpen)
                return;

            ManageMove();
            UpdateView();
        }

        private void ManageMove()
        {
            var dir = _myInputManager.GetDirectionInputInt();
            var inputX = dir.x;
            if (!_myInputManager.IsTriggerX())
                inputX = 0;

            var inputY = dir.y;
            if (!_myInputManager.IsTriggerY())
                inputY = 0;

            if (inputX == 0 && inputY == 0)
                return;

            //StateChange
            if (inputX < 0 && CurrentPos.x <= 0)
            {
                _deckEditStateModel.CurrentState = DeckEditState.MyDeck;
                return;
            }

            var nextPos = CurrentPos + new Vector2Int(inputX, -inputY);
            nextPos.x = Mathf.Clamp(nextPos.x, 0, 2);
            var index = PosToIndex(nextPos);
            var outOfLength = index < 0 || index >= _cardPoolModel.CurrentSortedCardPoolList.Count;
            if (outOfLength)
                return;

            if (_cardPoolListView.IsUpperOfView(nextPos.y))
            {
                _scrollView.ScrollOffset++;
            }

            if (_cardPoolListView.IsLowerOfView(nextPos.y))
            {
                _scrollView.ScrollOffset--;
            }

            CurrentPos = nextPos;

            AllAudioManager.PlaySe("CursorMove");

            _onMove.OnNext(Unit.Default);
        }

        private void UpdateView()
        {
            var view = _cardPoolListView.IconViewInstances[CurrentIndex];
            cursor.position = view.transform.position;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            cursor.gameObject.SetActive(isActive);
        }
    }
}