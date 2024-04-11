using System.Collections.Generic;
using System.Linq;
using Battle.Character.Enemy.Variables.Baltecia;
using Cysharp.Threading.Tasks;
using Others;
using Others.Utils;
using UniRx;

namespace Battle.Character.Enemy.Variables.TestSummonMan
{
    public class TestSummonManController : BossBase<TestSummonManController.State>
    {
        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            GameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => Loop().Forget())
                .AddTo(this);
        }

        private async UniTask Loop()
        {
            while (!commonCancellationTokenSource.IsCancellationRequested)
            {
                var nextState = State.Summon;


                LookPlayer();

                await PlayState(nextState);
            }
        }

        public enum State
        {
            Summon
        }
    }
}