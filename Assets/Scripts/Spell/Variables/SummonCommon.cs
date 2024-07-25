using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Spell.Variables
{
    public class SummonCommon : SpellBase
    {
        [SerializeField] private string servantKey;

        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(PlayerCore, 1, () => PlayerCore.transform.position));

            CharacterFactory.CreateAndInject(servantKey, PlayerCore, PlayerCore.transform.position);
        }
    }
}