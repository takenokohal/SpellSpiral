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
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Player, Color.white, 2,
                CalcPos));


            transform.position = CalcPos();
            attackHitController.gameObject.SetActive(true);
            effect.Play();

            await MyDelay(0.5f);
            attackHitController.gameObject.SetActive(false);

            await MyDelay(5f);

            Destroy(gameObject);
        }

        private Vector2 CalcPos()
        {
            return PlayerCore.Center.position + PlayerCore.CharacterRotation.Rotation * new Vector3(3, 0);
        }
    }
}