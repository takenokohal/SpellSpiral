using System.Linq;
using Battle.Attack;
using Battle.CommonObject.Bullet;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Servant
{
    public class MiniDragon : ServantBase
    {
        [SerializeField] private DirectionalBullet directionalBullet;
        [SerializeField] private Transform nozzle;

        [SerializeField] private float duration;
        [SerializeField] private float bulletSpeed;


        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            UniTask.Void(async () =>
            {
                while (!destroyCancellationToken.IsCancellationRequested)
                {
                    FireAnimationEvent();
                    await MyDelay(duration);
                }
            });
        }

        private void FireAnimationEvent()
        {
            var target =
                AllCharacterManager.AllCharacters
                    .Where(value => value.GetOwnerType() != GetOwnerType())
                    .OrderBy(value =>
                        Vector2.Distance(transform.position, value.transform.position)).First();

            CharacterRotation.Rotation = target.transform.position.x - transform.position.x;
            directionalBullet.CreateFromPrefab(nozzle.position, GetDirectionToTarget(target) * bulletSpeed);
        }
    }
}