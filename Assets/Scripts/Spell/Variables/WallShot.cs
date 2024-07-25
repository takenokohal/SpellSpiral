using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Spell.Variables
{
    public class WallShot : SpellBase
    {
        [SerializeField] private WallBullet bullet;

        [SerializeField] private float offset;
        [SerializeField] private float speed;


        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore, 2, CalcPos));

            var instance = (WallBullet)bullet.CreateFromPrefab(CalcPos(), CalcDir() * speed);

            instance.Init(AttackDatabase);
            instance.transform.right = CalcDir();
        }

        private Vector2 CalcPos()
        {
            return PlayerCore.Rigidbody.position + CalcDir() * offset;
        }

        private Vector3 CalcDir()
        {
            var target = AllCharacterManager.Boss;
            return target.Rigidbody.position - PlayerCore.Rigidbody.position;
        }
    }
}