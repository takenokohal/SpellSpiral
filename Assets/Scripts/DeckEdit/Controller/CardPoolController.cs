using Audio;
using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using DeckEdit.View.CardPool;
using Others;
using Others.Dialog;
using Others.Input;
using UniRx;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Controller
{
    public class CardPoolController : ITickable, IInitializable
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;

        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;

        [Inject] private readonly CardPoolModel _cardPoolModel;

        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;

        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;

        [Inject] private readonly CardPoolCursorView _cardPoolCursorView;
        [Inject] private readonly CardPoolListView _cardPoolListView;

        [Inject] private readonly DeckEditStateModel _deckEditStateModel;

        [Inject] private readonly MyDeckModel _myDeckModel;


        public void Initialize()
        {
            _deckEditStateModel.StateObservable.Subscribe(value =>
            {
                UniTask.Void(async () =>
                {
                    await UniTask.Yield(cancellationToken: _cardPoolCursorView.destroyCancellationToken);
                    _cardPoolCursorView.SetActive(value == DeckEditState.CardPool);
                });
            }).AddTo(_cardPoolCursorView);

            _cardPoolCursorView.OnMove.Subscribe(_ =>
            {
                var key = _cardPoolModel.CurrentSortedCardPoolList[_cardPoolCursorView.CurrentIndex];
                _currentSelectedSpell.SetSelectData(_spellDatabase.Find(key.Key));
            }).AddTo(_cardPoolCursorView);

            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => _cardPoolListView.IsInitialized);
                _cardPoolModel.ResetAndSort(CardPoolModel.SortType.Attribute);
            });
        }

        public void Tick()
        {
            if (_deckEditStateModel.CurrentState != DeckEditState.CardPool)
                return;

            if (AnyDialogIsOpen)
                return;

            ManageClick();
        }

        private void ManageClick()
        {
            if (!PlayerInput.actions["Yes"].WasPressedThisFrame())
                return;

            AllAudioManager.PlaySe("Select");
            var key = FindKey();
            _myDeckModel.Add(key);
        }

        private SpellKey FindKey()
        {
            var index = _cardPoolCursorView.CurrentIndex;

            var v = _cardPoolModel.CurrentSortedCardPoolList[index];
            return v;
        }
    }
}