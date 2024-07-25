using System;
using Audio;
using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using DeckEdit.View;
using DeckEdit.View.MyDeck;
using Others;
using Others.Dialog;
using Others.Input;
using UniRx;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Controller
{
    public class MyDeckController : ITickable, IInitializable
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;


        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;
        [Inject] private readonly MyDeckModel _myDeckModel;
        [Inject] private readonly SpellDatabase _spellDatabase;

        //    [Inject] private readonly OkDialog _okDialog;
        //    [Inject] private readonly YesNoDialog _yesNoDialog;
        //    private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;

        [Inject] private readonly ChoiceDialog _choiceDialog;

        [Inject] private readonly MyDeckCursorView _myDeckCursorView;
        [Inject] private readonly MyDeckListView _myDeckListView;
        [Inject] private readonly DeckEditStateModel _deckEditStateModel;

        [Inject] private readonly IDeckSaveDataPresenter _saveDataPresenter;

        public void Initialize()
        {
            _deckEditStateModel.StateObservable.Subscribe(value =>
            {
                UniTask.Void(async () =>
                {
                    await UniTask.Yield(cancellationToken: _myDeckCursorView.destroyCancellationToken);
                    _myDeckCursorView.SetActive(value == DeckEditState.MyDeck);
                });
            }).AddTo(_myDeckCursorView);

            _myDeckCursorView.OnMove.Subscribe(_ =>
            {
                var key = _myDeckModel.CurrentDeckList[_myDeckCursorView.CurrentIndex];
                var data = _spellDatabase.Find(key.Key);
                _currentSelectedSpell.SetSelectData(data);
            }).AddTo(_myDeckCursorView);

            _myDeckModel.OnUpdate.Subscribe(_ => { _myDeckListView.OnUpdate(_myDeckModel.CurrentDeckList); })
                .AddTo(_myDeckListView);

            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => _myDeckListView.IsInitialized);
                _myDeckModel.SetDeckData(_saveDataPresenter.LoadDeck());
            });
        }

        public void Tick()
        {
            if (_deckEditStateModel.CurrentState != DeckEditState.MyDeck)
                return;

            if (_choiceDialog.IsOpen)
                return;

            ManageClick();
        }

        private void ManageClick()
        {
            if (!PlayerInput.actions["Yes"].WasPressedThisFrame())
                return;

            AllAudioManager.PlaySe("Select");
            var key = FindKey();
            _myDeckModel.Remove(key);
        }

        private SpellKey FindKey()
        {
            var index = _myDeckCursorView.CurrentIndex;
            return _myDeckModel.CurrentDeckList[index];
        }
    }
}