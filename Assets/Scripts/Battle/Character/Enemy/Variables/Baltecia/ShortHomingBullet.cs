using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class ShortHomingBullet : EnemySequenceBase
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float drag;

        [SerializeField] private HomingBullet homingBullet;
        private HomingBulletFactory _homingBulletFactory;
        [SerializeField] private float recovery;

        [SerializeField] private float bulletFirstSpeed;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float changeSpeedValue;
        [SerializeField] private float bulletDuration;


        private void Start()
        {
            _homingBulletFactory = new HomingBulletFactory(homingBullet);
        }

        protected override async UniTask Sequence()
        {
            var rb = Parent.Rigidbody;
            rb.drag = drag;
            var theta = Random.Range(0, 2 * Mathf.PI);
            var velocity = moveSpeed * new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            rb.velocity = velocity;

            Parent.ToAnimationVelocity = velocity;

            for (int i = 0; i < 2; i++)
            {
                var i1 = i;
                UniTask.Void(async () =>
                {
                    await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Baltecia,
                        Color.red, 1, () => Parent.Center.position + CalcDir(i1)));

                    _homingBulletFactory.Create(new HomingBullet.Parameter()
                    {
                        ChangeSpeedValue = changeSpeedValue,
                        MaxSpeed = bulletSpeed,
                        Duration = bulletDuration,
                        Target = PlayerCore.Center,
                        FirstPos = Parent.Center.position + CalcDir(i1),
                        FirstVelocity = CalcDir(i1) * bulletFirstSpeed
                    });
                });
            }

            await MyDelay(recovery);
            Parent.ToAnimationVelocity = Vector2.zero;

            rb.drag = 0;
        }

        private Vector3 CalcDir(int i)
        {
            var toPlayer = GetDirectionToPlayer();
            var offset = i == 0 ? 1 : -1;
            return Quaternion.Euler(0, 0, 90 * offset) * toPlayer;
        }
    }
}