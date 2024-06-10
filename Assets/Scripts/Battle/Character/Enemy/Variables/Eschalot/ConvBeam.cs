using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class ConvBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private SingleHitBeam beamOrigin;


        [SerializeField] private float offsetDistance;
        [SerializeField] private float fullAngle;


        [SerializeField] private float duration;

        private readonly List<SingleHitBeam> _beams = new();

        private float _currentTime;
        public override EschalotState StateKey => EschalotState.ConvergenceBeam;

        private Vector2[] _currentDirections;
        private Vector2[] _currentPositions;

        private void Start()
        {
            _currentDirections = new Vector2[beamCount];
            _currentPositions = new Vector2[beamCount];
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

            await ReadyTimeCount();

            await MyDelay(duration);

            await MyDelay(1f);
        }

        private async UniTask ReadyTimeCount()
        {
            _currentTime = 0;
            while (_currentTime <= Parent.CharacterData.ChantTime - 0.2f)
            {
                _currentTime += Time.fixedDeltaTime;

                for (var i = 0; i < _currentDirections.Length; i++)
                {
                    var to = (Vector2)PlayerCore.Rigidbody.position - CalcPos(i);
                    _currentDirections[i] = Vector2.Lerp(CalcDir(i), to, _currentTime / Parent.CharacterData.ChantTime);

                    _currentPositions[i] = CalcPos(i);
                }

                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
            }

            await MyDelay(0.2f);
        }

        private async UniTaskVoid Shoot(int i)
        {
            ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => _currentPositions[i],
                1,
                () => _currentDirections[i])).Forget();


            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => _currentPositions[i]));


            var beam = _beams[i];
            beam.Activate(duration).Forget();
            beam.SetPositionAndDirection(_currentPositions[i], _currentDirections[i]);
        }


        private Vector2 CalcPos(int i)
        {
            var dir = CalcDir(i);
            var position = Parent.Rigidbody.position;
            var to = (Vector2)position + dir * offsetDistance;
            return to;
        }

        private Vector2 CalcDir(int i)
        {
            var dir1 = GetDirectionToPlayer();
            var shootArc = fullAngle / beamCount * i;

            var dir2 = Quaternion.Euler(0, 0, fullAngle / 2f) * dir1;
            var dir3 = Quaternion.Euler(0, 0, shootArc) * dir2;
            return Quaternion.Euler(0, 0, fullAngle / beamCount / 2f) * dir3;
        }
    }
}