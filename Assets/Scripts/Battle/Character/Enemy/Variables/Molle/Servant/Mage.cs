using System.Linq;
using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle.Servant
{
    public class Mage : ServantBase
    {
        [SerializeField] private float duration;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private DirectionalBullet bulletPrefab;
        

        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            Loop().Forget();
        }

        private async UniTaskVoid Loop()
        {
            while (!IsDead && !destroyCancellationToken.IsCancellationRequested)
            {
                var target = AllCharacterManager
                    .GetPlayerCharacters()
                    .OrderBy(value => value.CurrentLife)
                    .ThenBy(value => Vector3.Distance(value.transform.position, transform.position))
                    .First();

                var dir = GetDirectionToTarget(target);
                bulletPrefab.CreateFromPrefab(transform.position, dir * bulletSpeed);

                LookAtTarget(target);
              //  Animator.Play("Fire", 0, 0);
                await MyDelay(duration);
            }
        }
    }
}