using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class MissileCarnival : BossSequenceBase<BalteciaState>
    {
        //  [SerializeField] private HomingBullet homingBullet;

        [SerializeField] private HomingBullet homingBullet;


        [SerializeField] private float moveSpeed;


        [SerializeField] private int howMany;
        [SerializeField] private float shootingTime;

        [SerializeField] private float radius = 1;
        [SerializeField] private float radiusOffset = 0.02f;

        [SerializeField] private float maxSpeed;
        //     [SerializeField] private AnimationCurve animationCurve;

        [SerializeField] private float speedChangeRate;

        [SerializeField] private float bulletDuration;

        [SerializeField] private float recoveryTime;


        public override BalteciaState StateKey => BalteciaState.MissileCarnival;

        protected override async UniTask Sequence()
        {
            SpecialCameraSwitcher.SetSwitch(true);
            await Move();
            await Shoot();
            await MyDelay(recoveryTime);
            SpecialCameraSwitcher.SetSwitch(false);
        }

        private async UniTask Move()
        {
            Parent.Rigidbody.velocity = Vector3.zero;

            var dir = Vector3.zero - Parent.transform.position;
            Parent.ToAnimationVelocity = dir;
            await TweenToUniTask(Parent.transform.DOMove(Vector3.zero, moveSpeed)
                .SetSpeedBased()); 
            Parent.ToAnimationVelocity = Vector2.zero;
            await MyDelay(1f);
        }

        private async UniTask Shoot()
        {
            var v = howMany / 2;

            for (int i = 0; i < v; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    Generate(i, j).Forget();
                }


                var waitTime = shootingTime / v;
                await MyDelay(waitTime);
            }
        }

        private async UniTaskVoid Generate(int i, int j)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey, Color.magenta, 1,
                () => CalcPos(i, j)));

            homingBullet.CreateFromPrefab(new HomingBullet.Parameter()
            {
                FirstPos = CalcPos(i, j),
                Duration = bulletDuration,
                Target = PlayerCore.transform,
                MaxSpeed = maxSpeed,
                FirstVelocity = Vector2.zero,
                ChangeSpeedValue = speedChangeRate
            });
        }

        private Vector2 CalcPos(int i, int j)
        {
            var v = howMany / 2;

            var rad = (360f / v * i + 180f * j) * Mathf.Deg2Rad;

            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * (radius + radiusOffset * i);
        }
    }
}