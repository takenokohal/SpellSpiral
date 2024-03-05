using System.Linq;
using Battle.Character;
using Battle.Character.Enemy;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class HomingIces : SpellBase
    {
        [SerializeField] private HomingBullet homingBullet;

        [SerializeField] private int howMany;

        [SerializeField] private float changeSpeedValue;
        [SerializeField] private float firstSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float duration;


        private const float Arc = 90f;

        protected override async UniTaskVoid Init()
        {
            var target = AllEnemyManager.EnemyCores.OrderBy(value =>
                Vector3.Distance(value.Center.position, PlayerCore.Center.position)).First();

            for (int i = 0; i < howMany; i++)
            {
                Shoot(target, i).Forget();

                await MyDelay(0.1f);
            }

            await MyDelay(1f);
            Destroy(gameObject);
        }

        private async UniTask Shoot(EnemyCore target, int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Player, Color.white, 1f,
                () => CalcPos(target, i)));

            homingBullet.CreateFromPrefab(new HomingBullet.Parameter()
            {
                ChangeSpeedValue = changeSpeedValue,
                MaxSpeed = maxSpeed,
                Duration = duration,
                Target = target.Center,
                FirstPos = CalcPos(target, i),
                FirstVelocity = CalcDir(target, i) * firstSpeed
            });
        }


        private Vector2 CalcDir(EnemyCore target, int i)
        {
            var v1 = -GetDirectionToEnemy(target);
            var v2 = Quaternion.Euler(0, 0, -Arc / 2f) * v1;
            var v3 = Quaternion.Euler(0, 0, Arc / howMany * i) * v2;
            return v3;
        }

        private Vector2 CalcPos(EnemyCore target, int i)
        {
            var v3 = CalcDir(target, i);
            var pos = (Vector2)PlayerCore.Center.position + v3 * 0.5f;
            return pos;
        }
    }
}