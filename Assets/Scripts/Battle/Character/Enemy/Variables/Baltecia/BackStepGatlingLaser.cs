using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class BackStepGatlingLaser : BossSequenceBase<BalteciaState>
    {
        [SerializeField] private float backStepDrag;
        [SerializeField] private float backStepSpeed;
        [SerializeField] private float backStepDuration;

        [SerializeField] private float radius;
        [SerializeField] private int howManyIn1Side;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float shootingDuration;
        [SerializeField] private float shootingRecovery;


        [SerializeField] private GameObject laser;
        [SerializeField] private float recovery;


        [SerializeField] private DirectionalBullet directionalBulletPrefab;


        private void Start()
        {
            transform.SetParent(Parent.transform);
            transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        public override BalteciaState StateKey => BalteciaState.BackStepAndGatlingAndLaser;

        protected override async UniTask Sequence()
        {
            //移動
            Parent.Rigidbody.drag = backStepDrag;
            Parent.Rigidbody.velocity = -GetDirectionToPlayer() * backStepSpeed;

            Parent.ToAnimationVelocity = -GetDirectionToPlayer();
            await MyDelay(backStepDuration);


            Parent.ToAnimationVelocity = Vector2.zero;

            //射撃
            for (int i = 0; i < howManyIn1Side; i++)
            {
                Parent.LookPlayer();
                for (int j = 0; j < 2; j++)
                {
                    var i1 = i;
                    var j1 = j;
                    UniTask.Void(async () =>
                    {
                        var mcp = new MagicCircleParameters(CharacterKey, Color.red,
                            1f,
                            () => CalcPos(i1, j1));
                        await MagicCircleFactory.CreateAndWait(mcp);

                        directionalBulletPrefab.CreateFromPrefab(CalcPos(i1, j1),
                            ((Vector2)PlayerCore.Center.position - CalcPos(i1, j1)).normalized * bulletSpeed);
                    });
                }

                await MyDelay(shootingDuration / howManyIn1Side);
            }

            await MyDelay(shootingRecovery);

            /*
            Parent.LookPlayer();

            laser.transform.LookAt(PlayerCore.Center);
            laser.SetActive(true);
            await MyDelay(1f);
            laser.SetActive(false);
            await MyDelay(recovery - 1f);
            */
        }

        private Vector2 CalcPos(int i, int j)
        {
            var v = j == 0 ? 1 : -1;
            var offset = Quaternion.Euler(0, 0, 90f * v) * GetDirectionToPlayer() * radius / 2f;
            return Parent.transform.position + offset * (i * 0.2f) +
                   (Vector3)GetDirectionToPlayer() * ((i - 2) * 0.1f);
        }
    }
}