using System.Collections.Generic;
using System.Linq;
using Battle.System.Main;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UniRx;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class EschalotController : BossControllerBase<EschalotState>
    {
        private bool _halfLifeSpecialAttacked;

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
            //   await PlayState(DorothyState.FlowerGarden);

            var states = new List<EschalotState>
            {
                EschalotState.AreaBeam,
                EschalotState.ConvergenceBeam,
                EschalotState.DiffusionBeam
            };
            while (!commonCancellationTokenSource.IsCancellationRequested)
            {
                var nextState = states.Where(value => value != CurrentState).GetRandomValue();

                nextState = EschalotState.DiffusionBeam;
                LookPlayer();

                await PlayState(nextState);
            }
        }

        private bool HalfLifeCheck()
        {
            return CurrentLife <= CharacterData.Life / 2f && !_halfLifeSpecialAttacked;
        }
    }
}