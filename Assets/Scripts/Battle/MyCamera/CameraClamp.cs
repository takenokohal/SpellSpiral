using Cinemachine;
using UnityEngine;

namespace Battle.MyCamera
{
    public class CameraClamp : CinemachineExtension
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Vector2 maxPoint;


        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage,
            ref CameraState state,
            float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Finalize)
                return;

            var pos = state.RawPosition;

            var rightTop = cam.ViewportToWorldPoint(new Vector3(1f, 1f, -pos.z));

            var offset = rightTop - pos;

            pos.x = Mathf.Clamp(pos.x, -maxPoint.x + offset.x, maxPoint.x - offset.x);

            state.RawPosition = pos;
        }
    }
}