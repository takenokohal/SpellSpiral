using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class ResignBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private SingleHitBeam beamOrigin;

        [SerializeField] private float maxRotateSpeed;
        [SerializeField] private float minRotateSpeed;

        [SerializeField] private float duration;

        private readonly List<SingleHitBeam> _beams = new();
        private float[] _currentRotations;

        private bool _rotating;

        public override EschalotState StateKey => EschalotState.ResignBeam;

        private void Start()
        {
            _currentRotations = new float[beamCount];
            for (int i = 0; i < beamCount; i++)
            {
                var instance = Instantiate(beamOrigin, transform);
                _beams.Add(instance);
            }
        }

        protected override async UniTask Sequence()
        {
            await TweenToUniTask(Parent.Rigidbody.DOMove(Vector3.zero, 1f));

            _rotating = true;
            for (int i = 0; i < beamCount; i++)
            {
                Shoot(i).Forget();
            }

            await MyDelay(duration + Parent.CharacterData.ChantTime);

            _rotating = false;
            await MyDelay(1f);
        }

        private async UniTaskVoid Shoot(int i)
        {
            RotateLoop(i).Forget();

            ReadyEffectFactory.BeamCreateAndWait(
                new ReadyEffectParameter(
                    Parent,
                    () => CalcPos(i),
                    1,
                    () => _currentRotations[i])).Forget();


            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => CalcPos(i)));


            var beam = _beams[i];
            beam.Activate(duration).Forget();

            while (_rotating)
            {
                beam.SetPosition(CalcPos(i));
                beam.SetRotation(_currentRotations[i]);
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }

        private async UniTaskVoid RotateLoop(int i)
        {
            var rotSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
            rotSpeed *= Random.Range(0, 2) > 0 ? 1 : -1;
            _currentRotations[i] = Random.Range(0f, 360f);

            while (_rotating)
            {
                var dt = Time.fixedDeltaTime;
                _currentRotations[i] += rotSpeed * dt;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }
        }

        private Vector2 CalcPos(int i)
        {
            var angle = 360f / beamCount * i * Mathf.Deg2Rad;
            return (Vector2)Parent.Rigidbody.position + Vector2Extension.AngleToVector(angle);
        }
    }
}