using Battle.Character;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables.HighlanderSpell
{
    public class Stone : SpellBase
    {
        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Player, Color.white, 1,
                () => PlayerCore.Center.position));
        }
    }
}