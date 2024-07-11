using System;
using UniRx;

namespace DeckEdit.Model
{
    public class DeckEditStateModel : IDisposable
    {
        private readonly ReactiveProperty<DeckEditState> _currentState = new();

        public DeckEditState CurrentState
        {
            get => _currentState.Value;
            set => _currentState.Value = value;
        }

        public IObservable<DeckEditState> StateObservable => _currentState;

        public void Dispose()
        {
            _currentState?.Dispose();
        }
    }
}