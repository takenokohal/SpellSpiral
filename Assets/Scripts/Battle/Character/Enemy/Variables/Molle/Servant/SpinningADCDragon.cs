using System.Linq;
using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle.Servant
{
    public class SpinningAdcDragon : ServantBase
    {
        [SerializeField] private float spinningSpeed;
        [SerializeField] private float maxSpeed;

        [SerializeField] private float spinRadius;

        [SerializeField] private float duration;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private DirectionalBullet bulletPrefab;


        private float _currentSpinningTheta;

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
                Animator.Play("Fire", 0, 0);
                await MyDelay(duration);
            }
        }

        private void FixedUpdate()
        {
            _currentSpinningTheta += spinningSpeed * Time.fixedDeltaTime;
            _currentSpinningTheta = Mathf.Repeat(_currentSpinningTheta, Mathf.PI * 2f);

            var to = Master.transform.position +
                     new Vector3(Mathf.Cos(_currentSpinningTheta), Mathf.Sin(_currentSpinningTheta)) * spinRadius;
            to = Vector3.Lerp(Rigidbody.position, to, 0.2f);
            to = Vector3.MoveTowards(Rigidbody.position, to, maxSpeed * Time.fixedDeltaTime);
            Rigidbody.position = to;
        }
    }
}