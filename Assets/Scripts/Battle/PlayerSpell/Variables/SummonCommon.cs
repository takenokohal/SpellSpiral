using Battle.Attack;
using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class SummonCommon : SpellBase
    {
        [SerializeField] private ServantBase servantPrefab;

        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(PlayerCore, 1, () => PlayerCore.transform.position));

            CharacterFactory.CreateAndInject(servantPrefab, PlayerCore, PlayerCore.transform.position);
        }
    }
}