using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Others.Input
{
    public class MyInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput playerInput;
        //  [SerializeField] private PlayerInput battleInput;

        public PlayerInput PlayerInput => playerInput;
        //     public PlayerInput BattleInput => battleInput;

        private float _preInputX;
        private float _preInputY;
        private const float Threshold = 0.5f;

        private void LateUpdate()
        {
            var dir = GetDirectionInput();
            _preInputX = dir.x;
            _preInputY = dir.y;
        }

        public Vector2 GetDirectionInput()
        {
            return new Vector2(
                PlayerInput.actions["Horizontal"].ReadValue<float>(),
                PlayerInput.actions["Vertical"].ReadValue<float>());
        }

        public Vector2Int GetDirectionInputInt()
        {
            var dir = GetDirectionInput();
            return new Vector2Int(FloatToInt(dir.x), FloatToInt(dir.y));
        }


        public bool IsTriggerX()
        {
            var value = GetDirectionInput().x;
            switch (_preInputX)
            {
                case < Threshold when value >= Threshold:
                case > -Threshold when value <= -Threshold:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsTriggerY()
        {
            var value = GetDirectionInput().y;
            switch (_preInputY)
            {
                case < Threshold when value >= Threshold:
                case > -Threshold when value <= -Threshold:
                    return true;
                default:
                    return false;
            }
        }

        private static int FloatToInt(float value)
        {
            const float threshold = 0.3f;
            return value switch
            {
                > threshold => 1,
                < -threshold => -1,
                _ => 0
            };
        }
    }
}