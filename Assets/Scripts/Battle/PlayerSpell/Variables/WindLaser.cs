using Battle.Character;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class WindLaser : SpellBase
    {
        [SerializeField] private MultiHitLaser multiHitLaser;

        [SerializeField] private float duration;
        [SerializeField] private int hitCount;


        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                CharacterKey,
                Color.white,
                2f, CalcPos));
            
            var tmpPos = CalcPos();
            var tmpRot = PlayerCore.CharacterRotation.IsRight ? 0 : 180f;
            multiHitLaser.Activate(new MultiHitLaser.Parameter(
                () => tmpPos, () => tmpRot, duration, hitCount)).Forget();


            await MyDelay(5f);

            Destroy(gameObject);
        }

        private Vector2 CalcPos()
        {
            var pos = PlayerCore.transform.position + PlayerCore.CharacterRotation.Rotation * Vector3.right;
            return pos;
        }
    }
}