using System.Linq;
using Cinemachine;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Video;

namespace Test.Stage1Light
{
    public class BackGroundImage : MonoBehaviour
    {
        private void Start()
        {
            UniTask.Void(async () =>
            {
                await UniTask.Yield();
                var cam = Camera.allCameras.First(value =>
                    value.gameObject.layer == LayerMask.NameToLayer("BackGround"));

                var videoPlayer = FindObjectOfType<VideoPlayer>();
                videoPlayer.targetCamera = cam;
            });
            
        }
    }
}