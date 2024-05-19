using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class Surround : BossSequenceBase<DorothyState>
    {
        [SerializeField] private Vector2 radius;

        [SerializeField] private DirectionalBullet directionalBullet;
        [SerializeField] private float firstMoveSpeed;

        [SerializeField] private float moveDuration;

        [SerializeField] private float shootCoolTime;

        [SerializeField] private float bulletSpeed;

        private float _deltaTime;
        public override DorothyState StateKey => DorothyState.Surround;

        protected override async UniTask Sequence()
        {
            _deltaTime = 0;
            await Parent.Rigidbody.DOMove(new Vector3(0, radius.y), firstMoveSpeed);
            await Move();
            await MyDelay(2f);
        }

        private async UniTask Move()
        {
            var shootDeltaTime = 0f;
            while (_deltaTime < moveDuration)
            {
                Parent.LookPlayer();
                var v = _deltaTime / moveDuration;
                var angle = 2f * Mathf.PI * v;
                angle += Mathf.PI / 2f;
                Parent.Rigidbody.position = Vector2Extension.AngleToVector(angle) * radius;

                var dt = Time.fixedDeltaTime;
                _deltaTime += dt;
                shootDeltaTime += dt;

                if (shootDeltaTime > shootCoolTime)
                {
                    shootDeltaTime = 0f;
                    Shoot().Forget();
                }

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }

        private async UniTaskVoid Shoot()
        {
            var pos = Parent.Rigidbody.position;
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => pos));

            directionalBullet.CreateFromPrefab(pos, -pos.normalized * bulletSpeed);
        }
    }
}