using System.Linq;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Spell.Variables
{
    public class GatlingShoot : SpellBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float bulletSpeed;

        [SerializeField] private int howMany;
        [SerializeField] private float magicCircleOffset;
        [SerializeField] private float duration;


        protected override async UniTaskVoid Init()
        {
            for (int i = 0; i < howMany; i++)
            {
                Shoot(i).Forget();
                await MyDelay(duration / howMany);
            }

            await MyDelay(1f);
            Destroy(gameObject);
        }

        private async UniTaskVoid Shoot(int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(PlayerCore, 1,
                () => CalcPos(i)));

            var target = AllCharacterManager.GetEnemyCharacters()
                .OrderBy(value => value.CurrentLife)
                .ThenBy(value => Vector3.Distance(value.transform.position, PlayerCore.transform.position)).First();


            var pos = (Vector3)CalcPos(i);

            var velocity = (target.transform.position - pos).normalized * bulletSpeed;
            directionalBullet.CreateFromPrefab(pos, velocity);
        }

        private Vector2 CalcPos(int i)
        {
            var theta = 2 * Mathf.PI / howMany;
            theta *= i;
            var offset = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
            offset *= magicCircleOffset;
            return PlayerCore.transform.position + offset;
        }
    }
}