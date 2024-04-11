using Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class AudioTest : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                AllAudioManager.PlaySe("MagicCircle");
            }
        }
    }
}