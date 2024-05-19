using Audio;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class AudioTest : MonoBehaviour
    {

        private void Start()
        {
            Test().Forget();
        }
        

        private async UniTaskVoid Test()
        {
            await UniTask.Yield();
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                AllAudioManager.PlaySe("MagicShot");
                await UniTask.Delay(500, cancellationToken: destroyCancellationToken);
            }
        }
    }
}