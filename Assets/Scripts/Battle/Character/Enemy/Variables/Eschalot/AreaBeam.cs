using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class AreaBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private SingleHitBeam beamOrigin;


        [SerializeField] private float distance;
        [SerializeField] private float rotateSpeed;

        [SerializeField] private float duration;

        private readonly List<SingleHitBeam> _beams = new();

        private float _currentRotation;
        public override EschalotState StateKey => EschalotState.AreaBeam;


        private void Start()
        {
            for (int i = 0; i < beamCount; i++)
            {
                var instance = Instantiate(beamOrigin, transform);
                _beams.Add(instance);
            }
        }

        protected override async UniTask Sequence()
        {
            for (int i = 0; i < beamCount; i++)
            {
                Shoot(i).Forget();
            }

            await TimeCount();

            await MyDelay(1f);
        }

        private async UniTask TimeCount()
        {
            _currentRotation = 0;

            var t = 0f;
            while (t <= duration + Parent.CharacterData.ChantTime)
            {
                var dt = Time.fixedDeltaTime;
                _currentRotation += dt * rotateSpeed;
                t += dt;

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }

        private async UniTaskVoid Shoot(int i)
        {
            ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => CalcPos(i),
                1,
                () => (Quaternion.Euler(0, 0, 180f - 180f / beamCount / 2f) * CalcDir(i)))).Forget();


            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => CalcPos(i)));


            var beam = _beams[i];
            beam.Activate(duration).Forget();

            var t = 0f;
            while (t <= duration)
            {
                beam.SetPositionAndDirection(
                    CalcPos(i),
                    (Quaternion.Euler(0, 0, 180f - 180f / beamCount / 2f) * CalcDir(i)));

                t += Time.fixedDeltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }


        private Vector2 CalcPos(int i)
        {
            return CalcDir(i) * distance;
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