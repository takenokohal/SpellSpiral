using UnityEngine;
using UnityEngine.Video;

namespace Battle.System.BackGround
{
    public class BackGroundMovie : MonoBehaviour
    {
        private void Start()
        {
            var vp = GetComponent<VideoPlayer>();
            vp.targetCamera= Camera.main;
        }
    }
}