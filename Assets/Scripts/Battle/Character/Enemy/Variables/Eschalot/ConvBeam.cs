using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class ConvBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private SingleHitBeam beamOrigin;


        [SerializeField] private float offsetDistance;
        [SerializeField] private float fullAngle;


        [SerializeField] private float beamDuration;

        [SerializeField] private int count;
        [SerializeField] private float warpToPlayerDistance;


        private readonly List<SingleHitBeam> _beams = new();

        private float _currentTime;
        public override EschalotState StateKey => EschalotState.WarpConvergenceBeam;

        [SerializeField] private CharacterWarpController warpController;


        private void Start()
        {
            for (int i = 0; i < beamCount; i++)
            {
                var instance = Instantiate(beamOrigin, transform);
                _beams.Add(instance);
            }

            warpController.Init(Parent.Rigidbody, Parent.WizardAnimationController);
        }

        protected override async UniTask Sequence()
        {
            for (int i = 0; i < count; i++)
            {
                await Warp();
                for (int j = 0; j < beamCount; j++)
                {
                    Shoot(j).Forget();
                }

                //   await ReadyTimeCount();

                await MyDelay(beamDuration);
            }


            await MyDelay(1f);
        }

        private async UniTask Warp()
        {
            var playerPos = PlayerCore.Rigidbody.position;
            var r = Random.Range(0f, Mathf.PI * 2f);
            var offset = Vector2Extension.AngleToVector(r) * warpToPlayerDistance;
            var to = (Vector2)playerPos + offset;
            await warpController.PlayPositionWarp(new CharacterWarpController.PositionWarpParameter(to, 0.2f));
        }

        private async UniTaskVoid Shoot(int i)
        {
            var pos = CalcPos(i);
            var dir = (Vector2)PlayerCore.Rigidbody.position - pos;


            ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => pos,
                1,
                () => dir)).Forget();


            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => pos));


            var beam = _beams[i];
            beam.Activate(beamDuration).Forget();
            beam.SetPositionAndDirection(pos, dir);
        }


        private Vector2 CalcPos(int i)
        {
            var dir = CalcPosDir(i);
            var position = Parent.Rigidbody.position;
            var to = (Vector2)position + (dir * offsetDistance);
            return to;
        }

        private Vector2 CalcPosDir(int i)
        {
            var dir1 = -GetDirectionToPlayer();
            var shootArc = fullAngle / beamCount * i;

            var dir2 = Quaternion.Euler(0, 0, -fullAngle / 2f) * dir1;
            var dir3 = Quaternion.Euler(0, 0, shootArc) * dir2;
            return Quaternion.Euler(0, 0, fullAngle / beamCount / 2f) * dir3;
        }
    }
}