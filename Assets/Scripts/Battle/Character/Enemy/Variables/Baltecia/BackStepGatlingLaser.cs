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


        [SerializeField] private SingleHitBeam singleHitBeam;

        [SerializeField] private float recovery;


        [SerializeField] private DirectionalBullet directionalBulletPrefab;


        private Vector2 _currentBeamDir;

        public override BalteciaState StateKey => BalteciaState.BackStepAndGatlingAndLaser;

        protected override async UniTask Sequence()
        {
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Idle);
            WizardAnimationController.HorizontalSpeedValue = -1f;
            //移動
            Parent.Rigidbody.drag = backStepDrag;

            Parent.Rigidbody.velocity = -GetDirectionToPlayer() * backStepSpeed;

            Parent.ToAnimationVelocity = -GetDirectionToPlayer();
            await MyDelay(backStepDuration);


            Parent.ToAnimationVelocity = Vector2.zero;

            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Attack);

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
                        var mcp = new MagicCircleParameters(Parent,
                            1f,
                            () => CalcPos(i1, j1));

                        ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(
                            Parent,
                            () => CalcPos(i1, j1),
                            1,
                            () => CalcBulletDir(i1, j1))).Forget();
                        await MagicCircleFactory.CreateAndWait(mcp);

                        directionalBulletPrefab.CreateFromPrefab(CalcPos(i1, j1), CalcBulletDir(i1, j1) * bulletSpeed);
                    });
                }

                await MyDelay(shootingDuration / howManyIn1Side);
            }

            WizardAnimationController.HorizontalSpeedValue = 0f;


            await MyDelay(shootingRecovery);


            Parent.LookPlayer();
            await Beam();

            await MyDelay(recovery - 1f);
        }

        private async UniTask Beam()
        {
            ReadyEffectFactory
                .BeamCreateAndWait(new ReadyEffectParameter(Parent, CalcBeamPos, 2, () => _currentBeamDir))
                .Forget();
            RotateBeam().Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 2f, CalcBeamPos));
            singleHitBeam.Activate(1f).Forget();
            singleHitBeam.SetPositionAndDirection(CalcBeamPos(), _currentBeamDir);
        }

        private async UniTaskVoid RotateBeam()
        {
            _currentBeamDir = CalcBeamDir();
            var tmpTime = 0f;
            while (tmpTime < 1f)
            {
                _currentBeamDir = Vector2.Lerp(_currentBeamDir, CalcBeamDir(), 0.2f);
                tmpTime += Time.fixedDeltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }

        private Vector2 CalcBeamPos()
        {
            return (Vector2)Parent.Rigidbody.position + CalcBeamDir();
        }

        private Vector2 CalcBeamDir()
        {
            return GetDirectionToPlayer();
        }


        private Vector2 CalcPos(int i, int j)
        {
            var v = j == 0 ? 1 : -1;
            var offset = Quaternion.Euler(0, 0, 90f * v) * GetDirectionToPlayer() * radius / 2f;
            return (Vector2)Parent.Rigidbody.position +
                   (Vector2)offset * (i * 0.2f) +
                   GetDirectionToPlayer() * ((i - 2) * 0.2f)
                   - GetDirectionToPlayer();
        }

        private Vector2 CalcBulletDir(int i, int j)
        {
            var v = (Vector2)PlayerCore.transform.position - CalcPos(i, j);
            return v.normalized;
        }
    }
}