using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Others;
using Others.Utils;
using UniRx;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class BalteciaController : BossBase<BalteciaState>
    {
        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            gameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => Loop().Forget())
                .AddTo(this);
        }

        private async UniTask Loop()
        {
            await PlayState(BalteciaState.MissileCarnival);

            var states = new List<BalteciaState>
            {
                BalteciaState.TackleAndShotgun,
                BalteciaState.BackStepAndGatlingAndLaser,
                BalteciaState.CornerGatling3,
                BalteciaState.FlameSword,
                BalteciaState.Explosion,
                BalteciaState.ShortHomingBullet
            };

            while (!commonCancellationTokenSource.IsCancellationRequested)
            {
                var nextState = states.Where(value => value != CurrentState).GetRandomValue();


                LookPlayer();

                await PlayState(nextState);
            }
        }
    }
}