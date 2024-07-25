using Battle.System.Main;
using Cysharp.Threading.Tasks;
using Others;
using UniRx;

namespace Battle.Character.Enemy.Variables.TestSummonMan
{
    public class TestManController : BossControllerBase<TestManController.State>
    {
        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            BattleLoop.Event
                .Where(value => value == BattleEvent.BattleStart)
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