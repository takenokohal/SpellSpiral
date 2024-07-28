using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class BulletShoot : BossSequenceBase<EschalotState>
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private int shootCountPerSide;
        [SerializeField] private float positionOffset;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float bulletDuration;

        [SerializeField] private float backStepSpeed;
        [SerializeField] private float moveSpeed;

        [SerializeField] private float recovery;


        public override EschalotState StateKey => EschalotState.Shoot;

        protected override async UniTask Sequence()
        {
            Move().Forget();
            await ShootSeq();
            Parent.Rigidbody.drag = 0.5f;
            await MyDelay(recovery);
            Parent.Rigidbody.drag = 0;
            Parent.Rigidbody.velocity = Vector3.zero;
        }

        private async UniTask ShootSeq()
        {
            for (int i = 0; i < shootCountPerSide; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    var side = j == 0;
                    Shoot(i, side).Forget();

                    await MyDelay(bulletDuration / shootCountPerSide / 2f);
                }
            }
        }

        private async UniTaskVoid Shoot(int i, bool side)
        {
            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => CalcPos(i, side), 1, () => CalcDir(i, side))).Forget();

            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => CalcPos(i, side)));

            directionalBullet.CreateFromPrefab(CalcPos(i, side), CalcDir(i, side) * bulletSpeed);
        }

        private Vector2 CalcDir(int i, bool side)
        {
            return ((Vector2)PlayerCore.Rigidbody.position - CalcPos(i, side)).normalized;
        }

        private Vector2 CalcPos(int i, bool side)
        {
            var toP = GetDirectionToPlayer();
            var sideValue = side ? 1 : -1;
            var dir = Quaternion.Euler(0, 0, 90 * sideValue * (1f - (float)i / shootCountPerSide)) * toP;
            return dir * positionOffset + Parent.Rigidbody.position;
        }

        private async UniTaskVoid Move()
        {
            //BackStep
            Parent.Rigidbody.velocity = -GetDirectionToPlayer() * backStepSpeed;
            Parent.ToAnimationVelocity = -GetDirectionToPlayer();

            await MyDelay(Parent.CharacterData.ChantTime);


            Parent.Rigidbody.velocity = GetDirectionToPlayer() * moveSpeed;

            Parent.ToAnimationVelocity = GetDirectionToPlayer();
        }
    }
}