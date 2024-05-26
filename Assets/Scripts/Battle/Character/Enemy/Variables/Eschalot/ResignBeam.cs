using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class ResignBeam : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;
        [SerializeField] private MultiHitLaser laserOrigin;

        [SerializeField] private float maxRotateSpeed;
        [SerializeField] private float minRotateSpeed;

        [SerializeField] private float duration;

        private readonly List<MultiHitLaser> _lasers = new();
        private readonly List<ParticleSystem> _readyEffects = new();
        private readonly List<float> _currentRotations = new();

        [SerializeField] private ParticleSystem readyEffectOrigin;


        public override EschalotState StateKey => EschalotState.ResignBeam;

        protected override async UniTask Sequence()
        {
            for (int i = 0; i < beamCount; i++)
            {
                var instance = Instantiate(laserOrigin, transform);
                _lasers.Add(instance);
                _readyEffects.Add(Instantiate(readyEffectOrigin, transform));

                _currentRotations.Add(i);
                Shoot(i).Forget();
            }

            await MyDelay(duration);
            await MyDelay(1f);
        }

        private async UniTaskVoid Shoot(int i)
        {
            RotateLoop(i).Forget();
            UniTask.Void(async () =>
            {
                var ready = _readyEffects[i];
                ready.transform.position = CalcPos(i);
                var tmp = 0f;
                ready.Play();
                while (tmp <= 1f)
                {
                    ready.transform.rotation = Quaternion.Euler(0, 0, _currentRotations[i]);
                    tmp += Time.fixedDeltaTime;
                    await UniTask.Yield(PlayerLoopTiming.FixedUpdate, cancellationToken: destroyCancellationToken);
                }

                ready.Stop();
                ready.Clear();
            });

            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 1, () => CalcPos(i)));


            _lasers[i].Activate(new MultiHitLaser.Parameter(
                () => CalcPos(i),
                () => _currentRotations[i],
                duration - 1f,
                50)).Forget();
        }

        private async UniTaskVoid RotateLoop(int i)
        {
            var rotSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
            rotSpeed *= Random.Range(0, 2) > 0 ? 1 : -1;
            _currentRotations[i] = Random.Range(0f, 360f);


            var currentTime = 0f;
            while (currentTime <= duration)
            {
                var dt = Time.fixedDeltaTime;
                _currentRotations[i] += rotSpeed * dt;
                currentTime += dt;
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