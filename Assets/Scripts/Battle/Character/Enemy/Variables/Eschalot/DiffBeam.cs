using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class DiffBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private SingleHitBeam beamOrigin;


        [SerializeField] private float radius;
        [SerializeField] private float centerOffset;

        [SerializeField] private float fullAngle;


        [SerializeField] private float duration;

        [SerializeField] private int waveShootCount;

        [SerializeField] private DirectionalBullet bullet;
        [SerializeField] private float bulletSpeed;
        [SerializeField] private float shootRadius;

        [SerializeField] private CharacterWarpController characterWarpController;
        [SerializeField] private float xMax;
        [SerializeField] private float yMax;


        private readonly List<SingleHitBeam> _beams = new();

        private float _currentTime;
        public override EschalotState StateKey => EschalotState.DiffusionBeamAndWaveShoot;

        private void Start()
        {
            characterWarpController.Init(Parent.Rigidbody, Parent.WizardAnimationController);
            for (int i = 0; i < beamCount; i++)
            {
                var instance = Instantiate(beamOrigin, transform);
                _beams.Add(instance);
            }
        }

        protected override async UniTask Sequence()
        {
            var r = Random.Range(0, Mathf.PI * 2f);
            var v = Vector2Extension.AngleToVector(r) * new Vector2(xMax, yMax);
            await characterWarpController.PlayPositionWarp(new CharacterWarpController.PositionWarpParameter(v, 0.2f));
            for (int i = 0; i < beamCount; i++)
            {
                Shoot(i).Forget();
            }

            await WaveShootSeq();


            await MyDelay(1f);
        }

        private async UniTaskVoid Shoot(int i)
        {
            var pos = CalcPos(i);
            var dir = CalcDir(i);
            ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => pos,
                1,
                () => dir)).Forget();


            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => pos));


            var beam = _beams[i];
            beam.Activate(duration).Forget();
            beam.SetPositionAndDirection(pos, dir);
        }


        private Vector2 CalcPos(int i)
        {
            var dir = CalcDir(i);
            var position = (Vector2)Parent.Rigidbody.position - GetDirectionToPlayer() * centerOffset;
            var to = position + dir * radius;
            return to;
        }

        private Vector2 CalcDir(int i)
        {
            var dir1 = GetDirectionToPlayer();
            var shootArc = fullAngle / beamCount * i;

            var dir2 = Quaternion.Euler(0, 0, -fullAngle / 2f) * dir1;
            var dir3 = Quaternion.Euler(0, 0, shootArc) * dir2;
            return Quaternion.Euler(0, 0, fullAngle / beamCount / 2f) * dir3;
        }

        private async UniTask WaveShootSeq()
        {
            var tuples = new List<(Vector2 pos, Vector2 dir)>();
            for (int i = 0; i < waveShootCount; i++)
            {
                tuples.Add((CalcShootPos(i), CalcShootDir(i)));
            }

            for (int i = 0; i < waveShootCount; i++)
            {
                WaveShoot(tuples[i].pos, tuples[i].dir).Forget();
                await MyDelay(duration / waveShootCount);
            }
        }

        private async UniTaskVoid WaveShoot(Vector2 pos, Vector2 dir)
        {
            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => pos, 1, () => dir)).Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                Parent, 1, () => pos));

            bullet.CreateFromPrefab(pos, dir * bulletSpeed);
        }

        private Vector2 CalcShootPos(int i)
        {
            var dir = CalcShootDir(i);
            var position = (Vector2)Parent.Rigidbody.position - GetDirectionToPlayer() * centerOffset;
            var to = position + dir * shootRadius;
            return to;
        }

        private Vector2 CalcShootDir(int i)
        {
            var dir1 = -Parent.Rigidbody.position.normalized;
            var shootArc = fullAngle / waveShootCount * i;

            var dir2 = Quaternion.Euler(0, 0, -fullAngle / 2f) * dir1;
            var dir3 = Quaternion.Euler(0, 0, shootArc) * dir2;
            return Quaternion.Euler(0, 0, fullAngle / waveShootCount / 2f) * dir3;
        }
    }
}