using System.Linq;
using Battle.Character.Enemy;
using UnityEngine;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerMover : PlayerComponent
    {
        [Inject] private readonly AllEnemyManager _allEnemyManager;

        public static int HorizontalAnimKey { get; } = Animator.StringToHash("HorizontalSpeed");
        public static int VerticalAnimKey { get; } = Animator.StringToHash("VerticalSpeed");
        public Vector2 AnimationBlendValue { get; set; }

        protected override void Init()
        {
        }


        private void FixedUpdate()
        {
            if (!IsBattleStarted)
                return;

            if (PlayerParameter.Warping)
                return;


            SetRotation();

            TryNormalMove();
        }

        private void SetRotation()
        {
            if (_allEnemyManager.EnemyCores.Count == 1)
            {
                var enemy = _allEnemyManager.EnemyCores.First();
                var rot = enemy.transform.position.x - PlayerCore.transform.position.x;
                PlayerCore.CharacterRotation.Rotation = rot;
            }
            else
            {
                var input = GetInput().x;
                if (input != 0f)
                    PlayerCore.CharacterRotation.Rotation = input;
            }
        }

        private void TryNormalMove()
        {
            var input = GetInput();

            var rb = PlayerCore.Rigidbody;

            var to = new Vector2(input.x * PlayerCore.CharacterRotation.Rotation, input.y);

            AnimationBlendValue =
                Vector2.Lerp(AnimationBlendValue,
                    to, 0.1f);
            PlayerCore.Animator.SetFloat(HorizontalAnimKey, AnimationBlendValue.x);
            PlayerCore.Animator.SetFloat(VerticalAnimKey, AnimationBlendValue.y);

            var velocity = !PlayerParameter.QuickCharging
                ? Vector2.Lerp(rb.velocity, input * PlayerConstData.MoveSpeed, PlayerConstData.MoveLerpValue)
                : input * PlayerConstData.ChargingMoveSpeed;

            rb.velocity = velocity;
        }


        private Vector2 GetInput()
        {
            return PlayerCore.PlayerInput.actions["Move"].ReadValue<Vector2>();
        }
    }
}