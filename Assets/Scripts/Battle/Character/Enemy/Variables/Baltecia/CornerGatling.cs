using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class CornerGatling : BossSequenceBase<BalteciaState>
    {
        [SerializeField] private Vector2 cornerPosition;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveRecovery;

        [SerializeField] private int howManyIn1Loop;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float shootingDuration;

        [SerializeField] private float recovery;

        [SerializeField] private DirectionalBullet directionalBullet;


        public override BalteciaState StateKey => BalteciaState.CornerGatling3;

        protected override async UniTask Sequence()
        {
            Parent.Rigidbody.velocity= Vector3.zero;
            
            //Move
            var pos = Parent.transform.position;
            var cornerNormalize = new Vector2Int
            {
                x = pos.x >= 0 ? -1 : 1,
                y = pos.y >= 0 ? -1 : 1
            };

            var to = (Vector3)Vector2.Scale(cornerPosition, cornerNormalize);

            Parent.ToAnimationVelocity = to - Parent.transform.position;
            await TweenToUniTask(Parent.transform.DOMove(to, moveSpeed).SetSpeedBased());

            
            Parent.ToAnimationVelocity= Vector2.zero;
            await MyDelay(moveRecovery);

            Parent.CharacterRotation.Rotation = -to.x;


            //Shoot
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < howManyIn1Loop; j++)
                {
                    Generate(i, j, cornerNormalize).Forget();
                    await MyDelay(shootingDuration / howManyIn1Loop / 3f);
                }
            }

            await MyDelay(recovery);
        }

        private async UniTaskVoid Generate(int i, int j, Vector2Int cornerNormalize)
        {
            var loop2 = i % 2 == 1;
            var count = loop2 ? (howManyIn1Loop - j) : j;
            var theta = 90f / howManyIn1Loop * count * Mathf.Deg2Rad;
            var dir = new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
            dir.Scale(-cornerNormalize);


            var mcp = new MagicCircleParameters(Parent,  1f,
                () => CalcPos(i, j, dir));
            await MagicCircleFactory.CreateAndWait(mcp);

           directionalBullet.CreateFromPrefab(CalcPos(i, j, dir), dir * bulletSpeed);
        }

        private Vector2 CalcPos(int i, int j, Vector2 dir)
        {
            return (Vector2)Parent.transform.position + dir * (2f + i * 0.5f);
        }
    }
}