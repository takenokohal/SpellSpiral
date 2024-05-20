using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class FlowerGarden : BossSequenceBase<DorothyState>
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float xMax;
        [SerializeField] private float yMax;


        [SerializeField] private int flowerCount;
        [SerializeField] private float duration;

        [SerializeField] private int countPerPetal;
        [SerializeField] private int petalCount;
        [SerializeField] private float floweringDuration;
        [SerializeField] private float maxOffset;


        [SerializeField] private float recovery;

        [SerializeField] private float speed;

        [SerializeField] private float bulletLifeTime;

        public override DorothyState StateKey => DorothyState.FlowerGarden;

        protected override async UniTask Sequence()
        {
            SpecialCameraSwitcher.SetSwitch(true);
            await TweenToUniTask(Parent.Rigidbody.DOMove(new Vector3(0, 0), 1f));
            for (int i = 0; i < flowerCount; i++)
            {
                GenerateFlower().Forget();
                await MyDelay(duration / flowerCount);
            }

            SpecialCameraSwitcher.SetSwitch(false);
            await MyDelay(recovery);
        }

        private async UniTaskVoid GenerateFlower()
        {
            var posX = Random.Range(-xMax, xMax);
            var posY = Random.Range(-yMax, yMax);
            var pos = new Vector2(posX, posY);

            var randomAngle = Random.Range(0f, 360f);
            for (int i = 0; i < countPerPetal; i++)
            {
                for (int j = 0; j < petalCount; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Shoot(pos, i, j, k, randomAngle).Forget();
                    }
                }

                await MyDelay(floweringDuration / countPerPetal);
            }
        }

        private async UniTaskVoid Shoot(Vector2 pos, int i, int j, int k, float randomAngle)
        {
            var p = CalcPos(pos, i, j, k, randomAngle);
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 0.5f, () => p));
            var speedAdd = 1f - (float)i / countPerPetal;
            directionalBullet.CreateFromPrefab(p, CalcDir(i, j, k, randomAngle) * (speed + speedAdd), bulletLifeTime);
        }

        private Vector2 CalcPos(Vector2 pos, int i, int j, int k, float randomAngle)
        {
            var offset = -CalcDir(i, j, k, randomAngle);
            offset *= maxOffset * (1f - (countPerPetal - i) / (float)countPerPetal);
            return pos + offset;
        }


        private Vector2 CalcDir(int i, int j, int k, float randomAngle)
        {
            var angle = 360f / petalCount / countPerPetal * i / 2f;
            if (k == 0)
                angle *= -1;

            angle += 360f / petalCount * j;

            angle += randomAngle;

            angle *= Mathf.Deg2Rad;
            var direction = Vector2Extension.AngleToVector(angle);

            return direction;
        }
    }
}