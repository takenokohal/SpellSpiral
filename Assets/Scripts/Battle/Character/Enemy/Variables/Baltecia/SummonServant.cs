using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class SummonServant : EnemySequenceBase
    {
        protected override async UniTask Sequence()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                CharacterKey.Baltecia, 
                Color.red, 1,
                () => Parent.Center.position));
        }
    }
}