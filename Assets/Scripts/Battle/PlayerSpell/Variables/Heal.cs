using Audio;
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
            transform.SetParent(PlayerCore.transform);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);


            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore,
                1.5f, () => PlayerCore.transform.position));

            PlayerCore.CurrentLife += healValue;

            effect.Play();
            AllAudioManager.PlaySe("Heal");

            await MyDelay(1);
            Destroy(gameObject);
        }
    }
}