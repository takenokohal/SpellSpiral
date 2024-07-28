using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class WarpHeavyBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private CharacterWarpController characterWarpController;

        [SerializeField] private MultiHitLaser multiHitLaser;

        [SerializeField] private float xPos;

        [SerializeField] private float offset;

        [SerializeField] private float recovery;


        public override EschalotState StateKey => EschalotState.WarpHorizontalHeavyBeam;


        private void Start()
        {
            characterWarpController.Init(Parent.Rigidbody, Parent.WizardAnimationController);
        }

        protected override async UniTask Sequence()
        {
            var side = Random.Range(0, 2) == 0 ? 1 : -1;
            await characterWarpController.PlayPositionWarp(
                new CharacterWarpController.PositionWarpParameter(new Vector2(side * xPos, 0), 0.5f));

            ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => CalcPos(side),
                5,
                () => new Vector2(-side, 0))).Forget();

            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                Parent,
                5,
                () => CalcPos(side)));

            multiHitLaser.SetPosition(CalcPos(side));
            multiHitLaser.SetPositionAndRotation(CalcPos(side), side > 0 ? 180 : 0);
            await multiHitLaser.Activate(2f);

            await MyDelay(recovery);
        }

        private Vector2 CalcPos(int side)
        {
            return Parent.Rigidbody.position + new Vector3(-side * offset, 0);
        }
    }
}