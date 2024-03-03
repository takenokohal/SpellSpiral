using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Player
{
    public class PlayerWarp : PlayerComponent
    {
        [SerializeField] private ParticleSystem ps;
        [SerializeField] private GameObject graphic;


        private PlayerMover _playerMover;

        private CancellationTokenSource _cancellationTokenSource;

        protected override void Init()
        {
            _playerMover = GetComponent<PlayerMover>();
        }

        private void Update()
        {
            if (!IsBattleStarted) 
                return;
            
            
            if (!GetInput())
                return;

            if (PlayerParameter.Warping)
                return;

            PlayWarp().Forget();
        }

        private bool GetInput()
        {
            return PlayerCore.PlayerInput.actions["Step"].WasPressedThisFrame();
        }

        private Vector2 GetDirection()
        {
            return PlayerCore.PlayerInput.actions["Move"].ReadValue<Vector2>();
        }

        private async UniTask PlayWarp()
        {
            ResetToken();

            PlayerParameter.Invincible = true;
            PlayerParameter.Warping = true;
            PlayerParameter.QuickCharging = false;

            graphic.SetActive(false);
            ps.Play();
            PlayerCore.Rigidbody.drag = PlayerConstData.StepDrag;

            var dir = GetDirection();
            PlayerCore.Rigidbody.velocity = (Vector3)dir * PlayerConstData.StepSpeed;
            await UniTask.Delay((int)(PlayerConstData.StepDuration * 1000f), delayTiming: PlayerLoopTiming.FixedUpdate,
                cancellationToken: _cancellationTokenSource.Token);

            PlayerCore.Rigidbody.velocity = Vector3.zero;

            PlayerParameter.Invincible = false;
            PlayerParameter.Warping = false;

            graphic.SetActive(true);
            ps.Play();
            PlayerCore.Rigidbody.drag = 0;
        }

        private void ResetToken()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();
        }
    }
}