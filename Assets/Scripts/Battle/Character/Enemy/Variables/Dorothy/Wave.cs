using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class Wave : BossSequenceBase<DorothyState>
    {
        [SerializeField] private float moveSpeed;

        [SerializeField] private int petalCount;
        [SerializeField] private int loopCount;

        [SerializeField] private float duration;
        [SerializeField] private float bulletHighSpeed;
        [SerializeField] private float bulletLowSpeed;

        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float recovery;


        public override DorothyState StateKey => DorothyState.Wave;

        protected override async UniTask Sequence()
        {
            Parent.Rigidbody.velocity =
                Vector2Extension.AngleToVector(Random.Range(0f, 360f) * Mathf.Deg2Rad) * moveSpeed;
            for (int i = 0; i < loopCount; i++)
            {
                for (int k = 0; k < 2; k++)
                {
                    for (int j = 0; j < petalCount; j++)
                    {
                        Shoot(j, k).Forget();
                    }

                    await MyDelay(duration / loopCount / 2);
                }
            }
            await MyDelay(recovery);
            Parent.Rigidbody.velocity= Vector3.zero;
        }

        private async UniTaskVoid Shoot(int j, int k)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => CalcPos(j, k)));

            var speed = k == 0 ? bulletHighSpeed : bulletLowSpeed;
            directionalBullet.CreateFromPrefab(CalcPos(j, k), CalcDir(j, k) * speed);
        }

        private Vector2 CalcPos(int j, int k)
        {
            return CalcDir(j, k) + (Vector2)Parent.Rigidbody.position;
        }

        private Vector2 CalcDir(int j, int k)
        {
            var angle = 360f / petalCount * j;
            if (k == 0)
                angle += 360f / petalCount / 2f;

            angle *= Mathf.Deg2Rad;
            return Vector2Extension.AngleToVector(angle);
        }
    }
}