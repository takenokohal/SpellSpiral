using Battle.Character;
using Battle.Character.Player.Buff;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class BuffCommon : SpellBase
    {
        [SerializeField] private BuffKey buffKey;


        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(CharacterKey.Player,
                    Color.white, 1, () => PlayerCore.Center.position));

            PlayerBuff.SetBuff(new BuffParameter(buffKey, SpellData.EffectDuration));
        }
    }
}