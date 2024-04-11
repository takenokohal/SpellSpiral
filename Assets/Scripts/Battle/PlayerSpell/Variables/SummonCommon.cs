using Battle.Attack;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class SummonCommon : SpellBase
    {
        [SerializeField] private string servantKey;
        
        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(CharacterKey,
                    Color.white, 1, () => PlayerCore.transform.position));

            ServantFactory.Create(servantKey);
        }
    }
}