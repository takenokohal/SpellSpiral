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
        
        public PlayerInput PlayerInput=> playerInput;
   //     public PlayerInput BattleInput => battleInput;

    }
}