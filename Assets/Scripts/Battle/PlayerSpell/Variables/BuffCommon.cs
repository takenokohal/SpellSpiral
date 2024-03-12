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
                new MagicCircleParameters(CharacterKey,
                    Color.white, 1, () => PlayerCore.transform.position));

            PlayerBuff.SetBuff(new BuffParameter(buffKey, SpellData.EffectDuration));
        }
    }
}