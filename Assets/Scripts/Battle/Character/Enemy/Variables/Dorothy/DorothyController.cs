using Cysharp.Threading.Tasks;
using Others;
using UniRx;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class DorothyController : BossBase<DorothyState>
    {
        private bool _halfLifeSpecialAttacked;

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
                await PlayState(DorothyState.FlowerGarden);
            }
        }

        private bool HalfLifeCheck()
        {
            return CurrentLife <= CharacterData.Life / 2f && !_halfLifeSpecialAttacked;
        }
    }
}