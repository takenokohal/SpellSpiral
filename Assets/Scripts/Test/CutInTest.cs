using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class CutInTest : MonoBehaviour
    {
        [SerializeField] private GameObject normalCam;
        [SerializeField] private GameObject cutInCam;

        [SerializeField] private Animator anim;


        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                Animate().Forget();
        }

        private async UniTaskVoid Animate()
        {
            Debug.Log("AAAAA");
            cutInCam.gameObject.SetActive(true);
            anim.Play("CutIn", 0, 0);

            await UniTask.Delay(100);

            cutInCam.gameObject.SetActive(false);
        }
    }
}