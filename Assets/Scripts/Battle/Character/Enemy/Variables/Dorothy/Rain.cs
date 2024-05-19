using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class Rain : BossSequenceBase<DorothyState>
    {
        [SerializeField] private float xLength;
        [SerializeField] private float yPos;

        [SerializeField] private float bulletSpeed;

        [SerializeField] private DirectionalBullet directionalBullet;
        [SerializeField] private int count;

        [SerializeField] private float maxShootCoolTime;
        [SerializeField] private float coolTimeReduceValue;
        
        private float _currentShootCoolTime;

        public override DorothyState StateKey => DorothyState.Rain;

        protected override async UniTask Sequence()
        {
            _currentShootCoolTime = maxShootCoolTime;

            for (int i = 0; i < count; i++)
            {
                Shoot().Forget();

                _currentShootCoolTime *= coolTimeReduceValue;

                await MyDelay(_currentShootCoolTime);
            }

            await MyDelay(3);
        }

        private async UniTaskVoid Shoot()
        {
            var x = Random.Range(-xLength, xLength);
            var pos = new Vector2(x, yPos);
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => pos));

            var dir = Quaternion.Euler(0, 0, Random.Range(-45f, 45f)) * Vector3.down;

            directionalBullet.CreateFromPrefab(pos, dir * bulletSpeed);
        }
    }
}