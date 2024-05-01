using Cysharp.Threading.Tasks;
using Others;
using UniRx;

namespace Battle.Character.Enemy.Variables.TestSummonMan
{
    public class TestManController : BossBase<TestManController.State>
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
                var nextState = State.Spin;


                LookPlayer();

                await PlayState(nextState);
            }
        }

        public enum State
        {
            Spin,
            CycloneDanmaku
        }
    }
}