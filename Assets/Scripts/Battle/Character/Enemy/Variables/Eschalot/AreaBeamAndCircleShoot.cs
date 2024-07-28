using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class AreaBeamAndCircleShoot : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private SingleHitBeam beamOrigin;


        [SerializeField] private float distance;
        [SerializeField] private float rotateSpeed;
        [SerializeField] private float distanceSpeed;

        [SerializeField] private float duration;

        [SerializeField] private int bulletCount;
        [SerializeField] private float bulletDelay;
        [SerializeField] private float bulletDistance;
        [SerializeField] private float bulletSpeed;

        [SerializeField] private DirectionalBullet directionalBullet;
        [SerializeField] private CharacterWarpController characterWarpController;


        private readonly List<SingleHitBeam> _beams = new();

        private float _currentRotation;

        private float _currentDistance;
        public override EschalotState StateKey => EschalotState.AreaBeamAndCircleShoot;


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
            await characterWarpController.PlayPositionWarp(
                new CharacterWarpController.PositionWarpParameter(Vector2.zero, 0.2f));
            for (int i = 0; i < beamCount; i++)
            {
                Shoot(i).Forget();
            }

            ShootSeq().Forget();

            await TimeCount();

            await MyDelay(1f);
        }

        private async UniTaskVoid ShootSeq()
        {
            await MyDelay(bulletDelay);
            for (int i = 0; i < bulletCount; i++)
            {
                ShootBullet(i).Forget();
            }
        }

        private async UniTaskVoid ShootBullet(int i)
        {
            var rot = 2f * Mathf.PI / bulletCount * i;
            var dir = Vector2Extension.AngleToVector(rot);
            var pos = (Vector2)Parent.Rigidbody.position + dir * bulletDistance;
            ReadyEffectFactory.ShootCreateAndWait(new ReadyEffectParameter(Parent, () => pos, 1, () => dir))
                .Forget();
            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 1, () => pos));

            directionalBullet.CreateFromPrefab(pos, dir * bulletSpeed);
        }


        private async UniTask TimeCount()
        {
            _currentRotation = 0;
            _currentDistance = distance;

            var t = 0f;
            while (t <= duration + Parent.CharacterData.ChantTime)
            {
                var dt = Time.fixedDeltaTime;
                _currentRotation += dt * rotateSpeed;
                _currentDistance -= dt * distanceSpeed;
                t += dt;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }

        private async UniTaskVoid Shoot(int i)
        {
            for (int j = 0; j < 2; j++)
            {
                var j2 = j;
                ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                    Parent,
                    () => CalcPos(i),
                    1,
                    () => (
                        Quaternion.Euler(0, 0,
                            90f +
                            180 * j2)
                        * CalcDir(i)))).Forget();
            }

            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => CalcPos(i)));


            var beam = _beams[i];
            beam.Activate(duration).Forget();

            var t = 0f;
            while (t <= duration)
            {
                beam.SetPositionAndDirection(
                    CalcPos(i),
                    (
                        //  Quaternion.Euler(0, 0, 180f - 180f / beamCount / 2f) * 
                        Quaternion.Euler(0, 0, 90f) *
                        CalcDir(i)));

                t += Time.fixedDeltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }


        private Vector2 CalcPos(int i)
        {
            return CalcDir(i) * _currentDistance;
        }

        private Vector2 CalcDir(int i)
        {
            var angle = 360f / beamCount * i;
            angle += _currentRotation;
            angle *= Mathf.Deg2Rad;
            return Vector2Extension.AngleToVector(angle);
        }
    }
}