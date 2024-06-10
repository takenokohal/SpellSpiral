using Battle.Attack;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class TackleAndShotGun : BossSequenceBase<BalteciaState>
    {
        //   [SerializeField] private float drag;

        [SerializeField] private float backStepSpeed;
        //  [SerializeField] private float backStepDuration;

        [SerializeField] private float tackleSpeed;
        //   [SerializeField] private float tackleDuration;

        [SerializeField] private DirectionalBullet directionalBulletPrefab;
        [SerializeField] private int howManyShoot;
        [SerializeField] private float shootArc;
        [SerializeField] private float shootDuration;
        [SerializeField] private float shootSpeed;

        [SerializeField] private float recovery;


        [SerializeField] private ParticleSystem fireEffect;

        [SerializeField] private AttackHitController attackHitController;

        [SerializeField] private float moveTowardValue;


        private void Start()
        {
            UniTask.Void(async () =>
            {
                await UniTask.WaitWhile(() => !Parent.IsInitialized);

                transform.SetParent(Parent.transform);
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            });
        }


        public override BalteciaState StateKey => BalteciaState.TackleAndShotgun;

        protected override async UniTask Sequence()
        {
            await Tackle();

            LerpVelocity().Forget();
            Parent.ToAnimationVelocity = Vector2.zero;

            for (int i = 0; i < 2; i++)
            {
                var currentArc = 0f;
                //Shoot
                for (int j = 0; j < howManyShoot; j++)
                {
                    Shoot(i, currentArc).Forget();

                    currentArc += shootArc / howManyShoot;
                }

                await MyDelay(shootDuration);
            }

            // Parent.Rigidbody.drag = 0;

            await MyDelay(recovery);
        }

        private async UniTask Tackle()
        {
            //BackStep
            Parent.Rigidbody.velocity = -GetDirectionToPlayer() * backStepSpeed;
            fireEffect.Play();
            Parent.ToAnimationVelocity = -GetDirectionToPlayer();


            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 2,
                () => Parent.transform.position));

            attackHitController.gameObject.SetActive(true);

            //ドラッグ消してみる
            // Parent.Rigidbody.drag = drag;

            await Homing();

            //Parent.Rigidbody.drag = 0f;
            fireEffect.Stop();

            attackHitController.gameObject.SetActive(false);
        }

        private async UniTask Homing()
        {
            var firstDirection = GetDirectionToPlayer();
            var currentDirection = firstDirection;

            while (Vector2.Dot(firstDirection, GetDirectionToPlayer()) >= 0f)
            {
                currentDirection = Vector2.MoveTowards(currentDirection, GetDirectionToPlayer(), moveTowardValue);
                Parent.Rigidbody.velocity = currentDirection * tackleSpeed;
                fireEffect.transform.forward = currentDirection;

                Parent.ToAnimationVelocity = currentDirection;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: SequenceCancellationToken.Token);
            }
        }

        private async UniTaskVoid Shoot(int i, float currentArc)
        {
            Parent.Animator.Play("Attack", 0, 0);

            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(Parent, () => CalcPos(i, currentArc), 1,
                () => CalcDir(i, currentArc))).Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1,
                () => CalcPos(i, currentArc)));


            directionalBulletPrefab.CreateFromPrefab(CalcPos(i, currentArc), CalcDir(i, currentArc) * shootSpeed);
        }


        private Vector2 CalcDir(int i, float currentArc)
        {
            var dir1 = GetDirectionToPlayer();
            var dir2 = Quaternion.Euler(0, 0, -shootArc / 2f) * dir1;
            var dir3 = Quaternion.Euler(0, 0, currentArc) * dir2;
            return dir3;
        }

        private Vector2 CalcPos(int i, float currentArc)
        {
            var dir3 = CalcDir(i, currentArc);
            return (Vector2)Parent.transform.position + dir3 * (1 + i * 0.5f);
        }

        private async UniTaskVoid LerpVelocity()
        {
            var lerpValue = 0.1f;
            var rb = Parent.Rigidbody;
            while (rb.velocity.sqrMagnitude > 0.01f)
            {
                rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, lerpValue);


                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: SequenceCancellationToken.Token);
            }
        }
    }
}