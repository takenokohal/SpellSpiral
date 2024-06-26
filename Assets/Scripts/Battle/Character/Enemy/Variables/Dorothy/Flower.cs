using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class Flower : BossSequenceBase<DorothyState>
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float offsetDistance;

        [SerializeField] private int countPerPetal;
        [SerializeField] private int petalCount;
        [SerializeField] private float duration;

        [SerializeField] private float recovery;

        [SerializeField] private float speed;
        public override DorothyState StateKey => DorothyState.Flower;

        protected override async UniTask Sequence()
        {
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Charge);

            for (int i = 0; i < countPerPetal; i++)
            {
                for (int j = 0; j < petalCount; j++)
                {
                    for (int k = 0; k < 2; k++)
                    {
                        Shoot(i, j, k).Forget();
                    }
                }

                await MyDelay(duration / countPerPetal);
            }            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Idle);


            await MyDelay(recovery);
        }

        private async UniTaskVoid Shoot(int i, int j, int k)
        {
            ReadyEffectFactory
                .ShootCreateAndWait(new ReadyEffectParameter(Parent, () => CalcPos(i, j, k), 0.5f,
                    () => CalcDir(i, j, k))).Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => CalcPos(i, j, k)));
            directionalBullet.CreateFromPrefab(CalcPos(i, j, k), CalcDir(i, j, k) * speed);
        }


        private Vector2 CalcPos(int i, int j, int k)
        {
            var offset = -CalcDir(i, j, k) * 2f;
            offset *= 1f - (countPerPetal - i - 1) / (float)countPerPetal;
            return (Vector2)Parent.Rigidbody.position + offset;
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