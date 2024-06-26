using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class Cyclone : BossSequenceBase<DorothyState>
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float offsetDistance;

        [SerializeField] private int countPerLoop;
        [SerializeField] private int loopCount;
        [SerializeField] private float duration;

        [SerializeField] private float recovery;

        [SerializeField] private float speed;


        public override DorothyState StateKey => DorothyState.Cyclone;

        protected override async UniTask Sequence()
        {
            var toPos = (Vector2)Parent.Rigidbody.position +
                        Vector2Extension.AngleToVector(Random.Range(0, Mathf.PI * 2f) * 5f);
            await TweenToUniTask(Parent.Rigidbody.DOMove(toPos, 0.5f));
            
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Charge);
            for (int i = 0; i < loopCount; i++)
            {
                for (int j = 0; j < countPerLoop; j++)
                {
                    Shoot(j).Forget();
                    await MyDelay(duration / countPerLoop / loopCount);
                }
            }
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Idle);

            await MyDelay(recovery);
        }

        private async UniTaskVoid Shoot(int i)
        {
            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(
                Parent, () => CalcPos(i), 1, () => CalcDir(i))).Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => CalcPos(i)));

            directionalBullet.CreateFromPrefab(CalcPos(i), CalcDir(i) * speed);
        }

        private Vector2 CalcPos(int i)
        {
            return (Vector2)Parent.Rigidbody.position + CalcDir(i) * offsetDistance;
        }

        private Vector2 CalcDir(int i)
        {
            var angle = 2f * Mathf.PI / countPerLoop * i;

            var direction = Vector2Extension.AngleToVector(angle);

            return direction;
        }
    }
}