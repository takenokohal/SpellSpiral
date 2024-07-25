using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class SummonHighButterfly : BossSequenceBase<MolleState>
    {
        public override MolleState StateKey => MolleState.SummonHighButterfly;

        [SerializeField] private string highButterfly;

        protected override async UniTask Sequence()
        {
            Parent.transform.DOMove(new Vector3(0, 3), 1);
            
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 5,
                () => Vector2.zero));

            CharacterFactory.CreateAndInject(highButterfly, Parent, Vector2.zero);
        }
    }
}