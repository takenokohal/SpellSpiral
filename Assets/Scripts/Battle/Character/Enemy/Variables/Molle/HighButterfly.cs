using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class HighButterfly : ServantBase
    {
        [SerializeField] private float duration;
        [SerializeField] private int bulletCount;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private DirectionalBullet bulletPrefab;

        
        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            
            CharacterRotation.IsStop = true;
            Loop().Forget();
        }

        private async UniTaskVoid Loop()
        {
            while (!IsDead && !destroyCancellationToken.IsCancellationRequested)
            {
                for (int i = 0; i < bulletCount; i++)
                {
                    Shoot(i);
                }

                await MyDelay(duration);
            }
        }

        private void Shoot(int i)
        {
            var theta = (2 * Mathf.PI / bulletCount) * i;
            var dir = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));

            bulletPrefab.CreateFromPrefab(transform.position, dir * bulletSpeed);
        }
    }
}