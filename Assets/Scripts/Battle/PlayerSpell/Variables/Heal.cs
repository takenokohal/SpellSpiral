using Battle.Character;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class Heal : SpellBase
    {
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private int healValue;


        protected override async UniTaskVoid Init()
        {
            transform.SetParent(PlayerCore.Center);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);


            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Player, Color.white,
                1.5f, () => PlayerCore.Center.position));

            PlayerCore.PlayerParameter.Life += healValue;

            effect.Play();

            await MyDelay(1);
            Destroy(gameObject);
        }
    }
}