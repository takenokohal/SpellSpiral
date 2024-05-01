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
                new MagicCircleParameters(PlayerCore, 1, () => PlayerCore.transform.position));


            var ed = SpellData.EffectDuration;
            PlayerBuff.BuffParameters.Add(new BuffParameter(buffKey, SpellData.SpellKey, ed > 0 ? ed : null));

            await UniTask.Yield(destroyCancellationToken);

            Destroy(gameObject);
        }
    }
}