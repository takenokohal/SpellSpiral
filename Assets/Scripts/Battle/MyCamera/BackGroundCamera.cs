using Cinemachine;
using UnityEngine;

namespace Battle.MyCamera
{
    public class BackGroundCamera : CinemachineExtension
    {
        [SerializeField] private CinemachineVirtualCamera characterCamera;
        
        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage != CinemachineCore.Stage.Finalize)
            {
                return;
            }


            var offset = characterCamera.State.RawPosition;
            offset.z = 0;

            state.RawPosition += offset;
            state.Lens.FieldOfView = characterCamera.State.Lens.FieldOfView;
        }
    }
}