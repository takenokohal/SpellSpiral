using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Dorothy
{
    public class Cutter : BossSequenceBase<DorothyState>
    {
        [SerializeField] private SpinBullet spinBullet;

        [SerializeField] private float firstSpeed;
        [SerializeField] private float forcePower;

        [SerializeField] private int count;


        public override DorothyState StateKey => DorothyState.Cutter;

        protected override async UniTask Sequence()
        {
            for (int i = 0; i < count; i++)
            {
                Shoot(i).Forget();
            }

            await MyDelay(2);
        }

        private async UniTaskVoid Shoot(int i)
        {
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                () => CalcPos(i)));

            var forceDir = Quaternion.Euler(0, 0, 30) * -CalcDir(i);
            spinBullet.CreateFromPrefab(new SpinBullet.Parameter()
            {
                FirstPos = CalcPos(i), FirstVelocity = CalcDir(i) * firstSpeed, Force = forcePower * forceDir
            });
        }

        private Vector2 CalcDir(int i)
        {
            var angle = Mathf.PI * 2f * i / count;
            return Vector2Extension.AngleToVector(angle);
        }

        private Vector2 CalcPos(int i)
        {
            return (Vector2)Parent.Rigidbody.position + CalcDir(i);
        }
    }
}