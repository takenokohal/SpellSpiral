using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class SummonServant : BossSequenceBase<BalteciaState>
    {
        public override BalteciaState StateKey { get; }

        protected override async UniTask Sequence()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                CharacterKey, 
                Color.red, 1,
                () => Parent.transform.position));
        }
    }
}