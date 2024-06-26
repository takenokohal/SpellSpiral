using System.Collections.Generic;
using System.Linq;
using Battle.Character.Enemy.Variables.Baltecia;
using Cysharp.Threading.Tasks;
using Others;
using Others.Utils;
using UniRx;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class DorothyController : BossControllerBase<DorothyState>
    {
        private bool _halfLifeSpecialAttacked;

        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            BattleLoop.Event
                .Where(value => value ==BattleEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => Loop().Forget())
                .AddTo(this);
        }

        private async UniTask Loop()
        {
            await PlayState(DorothyState.FlowerGarden);
            var states = new List<DorothyState>()
            {
                DorothyState.Cutter,
                DorothyState.Cyclone,
                DorothyState.Flower,
                DorothyState.Rain,
                DorothyState.Surround,
                DorothyState.Wave
            };
            while (!commonCancellationTokenSource.IsCancellationRequested)
            {
                var nextState = states.Where(value => value != CurrentState).GetRandomValue();

                if (HalfLifeCheck())
                {
                    _halfLifeSpecialAttacked = true;
                    nextState = DorothyState.FlowerGarden;
                }

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