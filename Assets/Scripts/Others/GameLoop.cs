using System;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace Others
{
    public class GameLoop : IInitializable
    {
        public enum GameEvent
        {
            //シーン読み込み、開幕演出
            SceneStart,

            //開幕演出終了通知
            BattleStart,

            Win,

            Lose
        }

        private readonly ReactiveProperty<GameEvent> _event = new();
        public void SendEvent(GameEvent gameEvent) => _event.Value = gameEvent;

        public GameEvent CurrentState => _event.Value;

        public IObservable<GameEvent> Event => _event;

        public void Initialize()
        {
            Debug.Log(_event);
            SendEvent(GameEvent.SceneStart);
        }
    }
}