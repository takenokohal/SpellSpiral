using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Others.MyDebug
{
    public class TitleCapture : MonoBehaviour
    {
#if UNITY_EDITOR
        [Button]
        private void MoveToSceneCameraPosition()
        {
            var sceneCamera = SceneView.lastActiveSceneView.camera.transform;

            var mainCam = Camera.main.transform;
            mainCam.SetPositionAndRotation(sceneCamera.position, sceneCamera.rotation);
        }
#endif
    }
}