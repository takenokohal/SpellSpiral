using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace HomeScene
{
    public class HomeStateController : IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();

        public enum HomeState
        {
            MainMenu,
            MissionSelect,

            StateChanging
        }

        private readonly ReactiveProperty<HomeState> _currentState = new();

        public HomeState CurrentState
        {
            get => _currentState.Value;
            set
            {
                UniTask.Void(async () =>
                {
                    _currentState.Value = HomeState.StateChanging;
                    await UniTask.Delay(200, cancellationToken: _cancellationTokenSource.Token);
                    _currentState.Value = value;
                });
            }
        }

        public IObservable<HomeState> StateObservable => _currentState;

        public void Dispose()
        {
            _currentState?.Dispose();
            _cancellationTokenSource.Cancel();
        }
    }
}