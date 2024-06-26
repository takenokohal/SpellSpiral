using System.Collections.Generic;
using System.Linq;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class LongGatling : BossSequenceBase<BalteciaState>
    {
        [SerializeField] private DirectionalBullet directionalBullet;

        [SerializeField] private float moveSpeed;


        [SerializeField] private float bulletSpeed;

        [SerializeField] private int howMany;
        [SerializeField] private float magicCircleOffset;
        [SerializeField] private float duration;

        [SerializeField] private float recovery;


        protected override async UniTask Sequence()
        {
            var velocity = moveSpeed * Vector2Extension.AngleToVector(Random.Range(0f, Mathf.PI * 2f));

            Parent.Rigidbody.velocity = velocity;
            Parent.ToAnimationVelocity = velocity;

            for (int i = 0; i < howMany; i++)
            {
                Shoot(i).Forget();
                await MyDelay(duration / howMany);
            }

            await MyDelay(recovery);

            Parent.Rigidbody.velocity = Vector3.zero;
        }

        private async UniTaskVoid Shoot(int i)
        {
            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => CalcPos(i),
                1,
                () => (Vector2)PlayerCore.Rigidbody.position - CalcPos(i))).Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                () => CalcPos(i)));

            var pos = (Vector3)CalcPos(i);

            var velocity = (PlayerCore.Rigidbody.position - pos).normalized * bulletSpeed;
            directionalBullet.CreateFromPrefab(pos, velocity);
        }

        private Vector2 CalcPos(int i)
        {
            var theta = 2 * Mathf.PI / howMany;
            theta *= i;
            var offset = new Vector3(Mathf.Cos(theta), Mathf.Sin(theta));
            offset *= magicCircleOffset;
            return Parent.Rigidbody.position + offset;
        }

        public override BalteciaState StateKey => BalteciaState.LongGatling;
    }
}