using Battle.Character.Enemy.Variables.Molle.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class SummonFighterDragon : BossSequenceBase<MolleState>
    {
        public override MolleState StateKey => MolleState.SummonFighter;

        [SerializeField] private HomingServant homingServant;

        [SerializeField] private float recoveryTime;

        protected override async UniTask Sequence()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                CalcPos));
            
            
            var servant = ServantFactory.CreateAndInject(homingServant, Parent, CalcPos());

            await MyDelay(recoveryTime);
        }

        private Vector2 CalcPos()
        {
            return (Vector2)Parent.transform.position + GetDirectionToPlayer();
        }
    }
}