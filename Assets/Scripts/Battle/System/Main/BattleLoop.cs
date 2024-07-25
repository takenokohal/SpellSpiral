using System;
using UniRx;

namespace Battle.System.Main
{
    public class BattleLoop
    {
        private readonly ReactiveProperty<BattleEvent> _event = new();
        public void SendEvent(BattleEvent battleEvent) => _event.Value = battleEvent;

        public BattleEvent CurrentState => _event.Value;

        public IObservable<BattleEvent> Event => _event;
    }
}