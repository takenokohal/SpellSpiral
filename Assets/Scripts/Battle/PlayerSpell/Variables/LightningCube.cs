using Battle.Character;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class LightningCube : SpellBase
    {
        [SerializeField] private SingleHitLaser singleHitLaser;

        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Player, Color.white, 1f,
                CalcPos));

            var pos = CalcPos();


            singleHitLaser.Activate(new SingleHitLaser.Parameter(pos,
                new Vector2(PlayerCore.CharacterRotation.Rotation, 0), 0.2f)).Forget();

            await MyDelay(5f);

            Destroy(gameObject);
        }

        private Vector2 CalcPos()
        {
            var pos = PlayerCore.Center.position + PlayerCore.CharacterRotation.Rotation * Vector3.right;

            return pos;
        }
    }
}