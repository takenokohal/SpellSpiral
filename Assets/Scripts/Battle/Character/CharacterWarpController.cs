using Audio;
using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using Databases;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterWarpController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;

        private Rigidbody _rigidbody;
        private WizardAnimationController _wizardAnimationController;

        private bool _isInitialized;

        public class VelocityWarpParameter
        {
            public float Drag { get; }
            public Vector2 Velocity { get; }

            public float Duration { get; }

            public VelocityWarpParameter(float drag, Vector2 velocity, float duration)
            {
                Drag = drag;
                Velocity = velocity;
                Duration = duration;
            }
        }

        public class PositionWarpParameter
        {
            public Vector2 Pos { get; }

            public float Duration { get; }

            public PositionWarpParameter(Vector2 pos, float duration)
            {
                Pos = pos;
                Duration = duration;
            }
        }

        public void Init(Rigidbody rb, WizardAnimationController wizardAnimationController)
        {
            _rigidbody = rb;
            _wizardAnimationController = wizardAnimationController;

            transform.SetParent(_rigidbody.transform);
            transform.localPosition= Vector3.zero;
            _isInitialized = true;
        }

        public async UniTask PlayVelocityWarp(VelocityWarpParameter velocityWarpParameter)
        {
            if (!_isInitialized)
                Debug.LogError("NotInit");
            ps.Play();
            AllAudioManager.PlaySe("Warp");
            _rigidbody.drag = velocityWarpParameter.Drag;


            var velocity = velocityWarpParameter.Velocity;
            _rigidbody.velocity = velocity;


            _wizardAnimationController.SetGraphicVisible(false);
            _wizardAnimationController.HorizontalSpeedValue = velocity.normalized.x;

            await MyDelay(velocityWarpParameter.Duration);

            _rigidbody.velocity = Vector3.zero;


            _wizardAnimationController.SetGraphicVisible(true);
            ps.Play();
            _rigidbody.drag = 0;
        }

        public async UniTask PlayPositionWarp(PositionWarpParameter positionWarpParameter)
        {
            if (!_isInitialized)
                Debug.LogError("NotInit");


            ps.Play();
            AllAudioManager.PlaySe("Warp");

            _wizardAnimationController.SetGraphicVisible(false);

            await MyDelay(positionWarpParameter.Duration);

            _rigidbody.position = positionWarpParameter.Pos;

            _wizardAnimationController.SetGraphicVisible(true);
            ps.Play();

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);
        }

        private UniTask MyDelay(float duration) =>
            UniTask.Delay((int)(duration * 1000f), cancellationToken: destroyCancellationToken);
    }
}