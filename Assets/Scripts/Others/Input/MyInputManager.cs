using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Others.Input
{
    public class MyInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput uiInput;
        [SerializeField] private PlayerInput battleInput;
        
        public PlayerInput UiInput=> uiInput;
        public PlayerInput BattleInput => battleInput;

    }
}