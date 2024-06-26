using Audio;
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

        [SerializeField] private int warpCount;
        [SerializeField] private float warpDuration;

        [SerializeField] private float shootDuration;

        [SerializeField] private float recovery;

        [SerializeField] private CharacterWarpController characterWarpController;

        public override DorothyState StateKey => DorothyState.Cutter;

        private void Start()
        {
            characterWarpController.Init(Parent.Rigidbody, Parent.WizardAnimationController);
            UniTask.Void(async () =>
            {
                await UniTask.WaitWhile(() => !Parent.IsInitialized);

                transform.SetParent(Parent.transform);
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            });
        }

        protected override async UniTask Sequence()
        {
            for (int j = 0; j < warpCount; j++)
            {
                await Warp();

                for (int i = 0; i < count; i++)
                {
                    Shoot(i).Forget();
                }

                await MyDelay(shootDuration);
            }
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Idle);

            await MyDelay(recovery);
        }

        private async UniTaskVoid Shoot(int i)
        {
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Attack);
            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => CalcPos(i),
                0.5f,
                () => CalcDir(i))).Forget();
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
            return (Vector2)Parent.Rigidbody.position + CalcDir(i) * 0.5f;
        }

        private async UniTask Warp()
        {
            var pos = Random.insideUnitCircle * 5;
            await characterWarpController.PlayPositionWarp(
                new CharacterWarpController.PositionWarpParameter(pos, warpDuration));
        }
    }
}