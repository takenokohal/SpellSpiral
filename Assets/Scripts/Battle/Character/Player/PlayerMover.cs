using System.Linq;
using Battle.Character.Enemy;
using UnityEngine;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerMover : PlayerComponent
    {
        [Inject] private readonly AllCharacterManager _allCharacterManager;

        public static int HorizontalAnimKey { get; } = Animator.StringToHash("HorizontalSpeed");
        public static int VerticalAnimKey { get; } = Animator.StringToHash("VerticalSpeed");
        public Vector2 AnimationBlendValue { get; set; }

        protected override void Init()
        {
        }


        private void FixedUpdate()
        {
            if (!IsInitialized)
                return;
            if (!IsBattleStarted)
                return;

            if (PlayerParameter.Warping)
                return;

            //チャージ中動けないように
            if (PlayerParameter.QuickCharging)
                return;


            SetRotation();

            TryNormalMove();
        }

        private void SetRotation()
        {
            var enemyXPoses = _allCharacterManager.GetEnemyCharacters().Select(value => value.transform.position.x)
                .ToArray();
            var playerXPos = PlayerCore.transform.position.x;
            if (enemyXPoses.All(value => value - playerXPos <= 0) || enemyXPoses.All(value => value - playerXPos > 0))
            {
                var enemy = _allCharacterManager.AllCharacters.First();
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

            /*
            var velocity = !PlayerParameter.QuickCharging
                ? Vector2.Lerp(rb.velocity, input * PlayerConstData.MoveSpeed, PlayerConstData.MoveLerpValue)
                : input * PlayerConstData.ChargingMoveSpeed;
                */

            var velocity = Vector2.Lerp(rb.velocity, input * PlayerConstData.MoveSpeed, PlayerConstData.MoveLerpValue);

            rb.velocity = velocity;
        }


        private Vector2 GetInput()
        {
            return PlayerCore.PlayerInput.actions["Move"].ReadValue<Vector2>();
        }
    }
}