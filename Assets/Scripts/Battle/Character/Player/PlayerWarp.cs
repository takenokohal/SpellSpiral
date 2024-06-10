using System.Threading;
using Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Player
{
    public class PlayerWarp : PlayerComponent
    {
        [SerializeField] private CharacterWarpController characterWarpController;

        private bool CantWarp => PlayerParameter.Warping || PlayerParameter.SpellChanting;

        protected override void Init()
        {
            characterWarpController.Init(PlayerCore.Rigidbody, WizardAnimationController);
        }

        private void Update()
        {
            if (!IsBattleStarted)
                return;


            if (!GetInput())
                return;

            if (CantWarp)
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
            PlayerParameter.Invincible = true;
            PlayerParameter.Warping = true;
            PlayerParameter.QuickCharging = false;

            var dir = GetDirection();

            await characterWarpController.PlayWarp(
                new CharacterWarpController.WarpParameter(
                    PlayerConstData.StepDrag,
                    PlayerConstData.StepSpeed * dir,
                    PlayerConstData.StepDuration));


            PlayerParameter.Invincible = false;
            PlayerParameter.Warping = false;
        }
    }
}