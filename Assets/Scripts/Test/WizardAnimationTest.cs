/*using Battle.Character;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class WizardAnimationTest : MonoBehaviour
    {
        private WizardAnimationController _wizardAnimationController;
        [SerializeField] private PlayerInput playerInput;


        private void Start()
        {
            _wizardAnimationController = new WizardAnimationController(GetComponent<Animator>());
            Test().Forget();
        }

        private async UniTaskVoid Test()
        {
            await UniTask.WaitUntil(() => Keyboard.current.spaceKey.wasPressedThisFrame);
            
            _wizardAnimationController.PlayAttack();

        }
    }
}*/