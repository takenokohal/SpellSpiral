using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using Unity.VisualScripting;
using UnityEngine;

namespace Test
{
    
    public class FlowerBulletTest : MonoBehaviour
    {
        [SerializeField] private SpinBullet bullet;

        [SerializeField] private float offsetDistance;

        [SerializeField] private int countPerPetal;
        [SerializeField] private int petalCount;
        [SerializeField] private float duration;

        [SerializeField] private float recovery;

        [SerializeField] private float speed;


        //   [SerializeField] private bool isVelocityType;

        private void Start()
        {
            Seq().Forget();
        }


        private async UniTaskVoid Seq()
        {
            await UniTask.Yield();

            while (!destroyCancellationToken.IsCancellationRequested)
            {
                for (int i = 0; i < countPerPetal; i++)
                {
                    for (int j = 0; j < petalCount; j++)
                    {
                        for (int k = 0; k < 2; k++)
                        {
                            Shoot(i, j, k);
                        }
                    }

                    await MyDelay(duration / countPerPetal);
                }

                var v = recovery;
                await MyDelay(v);
            }
        }

        private UniTask MyDelay(float time)
        {
            return UniTask.Delay(
                (int)(time * 1000f), cancellationToken: destroyCancellationToken);
        }

        private void Shoot(int i, int j, int k)
        {
            var sp = speed * (1f - (float)i / countPerPetal);
            var power = speed * (float)i / countPerPetal;

            bullet.CreateFromPrefab(new SpinBullet.Parameter()
            {
                FirstPos = CalcPos(i, j, k),
                FirstVelocity = CalcDir(i, j, k) * sp,
                Force = CalcDir(i, j, k) * power
            });
        }


        private Vector2 CalcPos(int i, int j, int k)
        {
            return (Vector2)transform.position + CalcDir(i, j, k) * offsetDistance;
        }

        private Vector2 CalcDir(int i, int j, int k)
        {
            var angle = 360f / petalCount / countPerPetal * i / 2f;
            if (k == 0)
                angle *= -1;

            angle += 360f / petalCount * j;

            angle *= Mathf.Deg2Rad;
            var direction = Vector2Extension.AngleToVector(angle);

            return direction;
        }
    }
}