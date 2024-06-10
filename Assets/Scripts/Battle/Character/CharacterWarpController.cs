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

        public class WarpParameter
        {
            public float Drag { get; }
            public Vector2 Velocity { get; }

            public float Duration { get; }

            public WarpParameter(float drag, Vector2 velocity, float duration)
            {
                Drag = drag;
                Velocity = velocity;
                Duration = duration;
            }
        }

        public void Init(Rigidbody rb, WizardAnimationController wizardAnimationController)
        {
            _rigidbody = rb;
            _wizardAnimationController = wizardAnimationController;
        }

        public async UniTask PlayWarp(WarpParameter warpParameter)
        {
            ps.Play();
            AllAudioManager.PlaySe("Warp");
            _rigidbody.drag = warpParameter.Drag;


            var velocity = warpParameter.Velocity;
            _rigidbody.velocity = velocity;


            _wizardAnimationController.SetGraphicVisible(false);
            _wizardAnimationController.HorizontalSpeedValue = velocity.normalized.x;

            await MyDelay(warpParameter.Duration);

            _rigidbody.velocity = Vector3.zero;


            _wizardAnimationController.SetGraphicVisible(true);
            ps.Play();
            _rigidbody.drag = 0;
        }

        private UniTask MyDelay(float duration) =>
            UniTask.Delay((int)(duration * 1000f), cancellationToken: destroyCancellationToken);
    }
}