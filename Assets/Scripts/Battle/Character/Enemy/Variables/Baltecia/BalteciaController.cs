using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Others.Utils;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class BalteciaController : BossBase<BalteciaState>
    {
        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            Loop().Forget();
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