using Audio;
using Battle.Attack;
using Battle.Character;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class Explosion : SpellBase
    {
        [SerializeField] private AttackHitController attackHitController;
        [SerializeField] private ParticleSystem effect;


        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore, 2,
                CalcPos));


            transform.position = CalcPos();
            attackHitController.gameObject.SetActive(true);
            effect.Play();
            AllAudioManager.PlaySe("Explosion");


            await MyDelay(0.5f);
            attackHitController.gameObject.SetActive(false);

            await MyDelay(5f);

            Destroy(gameObject);
        }

        private Vector2 CalcPos()
        {
            return PlayerCore.transform.position;
        }
    }
}