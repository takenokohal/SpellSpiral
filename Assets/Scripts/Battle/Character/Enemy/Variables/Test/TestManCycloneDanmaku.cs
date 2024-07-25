using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.TestSummonMan
{
    public class TestManCycloneDanmaku : BossSequenceBase<TestManController.State>
    {
        [SerializeField] private DirectionalBullet bullet;
        [SerializeField] private float firstSpeed;
        [SerializeField] private int count;
        [SerializeField] private float duration;

        [SerializeField] private float recovery;


        public override TestManController.State StateKey => TestManController.State.Spin;

        protected override async UniTask Sequence()
        {
            for (int i = 0; i < count; i++)
            {
                Shoot(i).Forget();
                await MyDelay(duration / count);
            }

            await MyDelay(recovery);
        }

        private async UniTaskVoid Shoot(int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => CalcPos(i)));
            bullet.CreateFromPrefab(CalcPos(i), CalcDir(i) * firstSpeed);

            /*
            spinBullet.CreateFromPrefab(new SpinBullet.Parameter()
            {
                FirstPos = CalcPos(i), FirstVelocity = CalcDir(i) * firstSpeed, SpinAngle = -spinAngle,
                SpinPower = spinPower
            });

*/
        }

        private Vector3 CalcPos(int i)
        {
            return Parent.Rigidbody.position + CalcDir(i);
        }

        private Vector3 CalcDir(int i)
        {
            var theta = 2f * Mathf.PI / count * i;
            return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
        }
    }
}