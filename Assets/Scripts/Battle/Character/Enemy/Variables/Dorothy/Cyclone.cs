using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
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
            for (int i = 0; i < loopCount; i++)
            {
                for (int j = 0; j < countPerLoop; j++)
                {
                    Shoot(j).Forget();
                    await MyDelay(duration / countPerLoop / loopCount);
                }
            }

            await MyDelay(recovery);
        }

        private async UniTaskVoid Shoot(int i)
        {
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