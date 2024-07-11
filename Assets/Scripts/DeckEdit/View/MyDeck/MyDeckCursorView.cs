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

namespace DeckEdit.View.MyDeck
{
    public class MyDeckCursorView : MonoBehaviour
    {
        [Inject] private readonly MyInputManager _myInputManager;
        [Inject] private readonly MyDeckListView _myDeckListView;

        [SerializeField] private int xLength;
        [SerializeField] private int yLength;

        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;
        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;
        public Vector2Int CurrentPos { get; private set; }
        [Inject] private readonly MyDeckModel _myDeckModel;
        [Inject] private readonly DeckEditStateModel _deckEditStateModel;

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

        private void Start()
        {
            _myDeckModel.OnUpdate.Subscribe(_ =>
            {
                if (CurrentIndex >= GetCurrentDeckLength())
                    CurrentIndex--;
            }).AddTo(this);
        }

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

            if (CurrentPos.y == 0 && inputY == 1)
            {
                _deckEditStateModel.CurrentState = DeckEditState.Highlander;
                return;
            }


            var nextPos = CurrentPos + new Vector2Int(inputX, -inputY);

            var length = GetCurrentDeckLength();
            var outOfLength = PosToIndex(nextPos) >= length;
            outOfLength |= nextPos.x >= xLength ;
            outOfLength |= nextPos.x < 0;
            outOfLength |= nextPos.y >= yLength;
            outOfLength |= nextPos.y < 0;


            if (outOfLength)
            {
                if (inputX == 1)
                    _deckEditStateModel.CurrentState = DeckEditState.CardPool;
                
                return;
            }

            CurrentPos = nextPos;

            AllAudioManager.PlaySe("CursorMove");

            _onMove.OnNext(Unit.Default);
        }


        private int GetCurrentDeckLength()
        {
            return _myDeckModel.CurrentDeckList.Count;
        }


        private void UpdateView()
        {
            var view = _myDeckListView.IconViewInstances[CurrentIndex];
            cursor.position = view.transform.position;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            cursor.gameObject.SetActive(isActive);
        }
    }
}