using System;
using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class MultiHitLaser : MonoBehaviour
    {
        [SerializeField] private AttackHitController attackHitControllerInChildren;
        [SerializeField] private ParticleSystem effect;

        public class Parameter
        {
            public Func<Vector2> Pos { get; }
            public Func<float> Rotation { get; }

            public int HitCount { get; }
            public float ActiveTime { get; }

            public Parameter(Func<Vector2> pos, Func<float> rotation, float activeTime, int hitCount)
            {
                Pos = pos;
                Rotation = rotation;
                ActiveTime = activeTime;
                HitCount = hitCount;
            }

            public Parameter(Func<Vector2> pos, Func<Vector2> direction, float activeTime, int hitCount)
            {
                Pos = pos;
                Rotation = delegate
                {
                    var dir = direction.Invoke();
                    var rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                    return rot;
                };
                ActiveTime = activeTime;
                HitCount = hitCount;
            }
        }

        public async UniTaskVoid Activate(Parameter parameter)
        {
            effect.Play();
            HitLoop(parameter.ActiveTime, parameter.HitCount).Forget();

            var timeCount = 0f;

            var t = transform;
            while (timeCount < parameter.ActiveTime)
            {
                t.position = parameter.Pos.Invoke();
                t.rotation = Quaternion.Euler(0, 0, parameter.Rotation.Invoke());

                timeCount += Time.fixedDeltaTime;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, destroyCancellationToken);
            }

            effect.Stop();
        }

        private async UniTask HitLoop(float activeTime, int hitCount)
        {
            var deltaTime = activeTime / hitCount;
            for (int i = 0; i < hitCount; i++)
            {
                attackHitControllerInChildren.gameObject.SetActive(true);
                AllAudioManager.PlaySe("Laser");
                await UniTask.Delay((int)(deltaTime * 1000f), delayTiming: PlayerLoopTiming.FixedUpdate,
                    cancellationToken: destroyCancellationToken);
                attackHitControllerInChildren.gameObject.SetActive(false);
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate, destroyCancellationToken);
            }
        }
    }
}