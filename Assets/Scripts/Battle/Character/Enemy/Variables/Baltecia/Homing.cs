/*using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Enemy.Variables.Baltecia
{
    public class Homing : EnemySequenceBase
    {
        [SerializeField] private float speed;
        [SerializeField] private float maxTime = 3;
        [SerializeField] private float goalDistance;

        private float _currentTime;

        public override async UniTask SequenceStart()
        {
            while (_currentTime <= maxTime && !IsNear())
            {
                var dt = Time.fixedDeltaTime;
                Parent.Rigidbody.velocity = Vector3.Lerp(
                    Parent.Rigidbody.velocity, GetDirectionToPlayer() * speed, dt*5f);
                _currentTime += dt;
                await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
            }
        }

        private bool IsNear()
        {
            return Vector3.Distance(PlayerCore.Center.position, Parent.Center.position) < goalDistance;
        }
    }
}*/