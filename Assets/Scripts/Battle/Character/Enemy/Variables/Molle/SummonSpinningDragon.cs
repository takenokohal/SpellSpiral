using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class SummonSpinningDragon : BossSequenceBase<MolleState>
    {
        [SerializeField] private string servantPrefab;

        [SerializeField] private float recoveryTime;

        public override MolleState StateKey => MolleState.SummonADC;

        protected override async UniTask Sequence()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                CalcPos));
            var servant = CharacterFactory.CreateAndInject(servantPrefab, Parent, CalcPos());

            await MyDelay(recoveryTime);
        }

        private Vector2 CalcPos()
        {
            return Parent.transform.position + new Vector3(Parent.CharacterRotation.Rotation * -1.5f, 1);
        }
    }
}