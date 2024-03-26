using Cinemachine;
using UnityEngine;

namespace Battle.MyCamera
{
    public class BackGroundCamera : CinemachineExtension
    {
        private bool _isInitialized;
        private CinemachineVirtualCamera _characterCamera;

        public void Initialize(CinemachineVirtualCamera characterCamera)
        {
            _characterCamera = characterCamera;
            _isInitialized = true;
        }

        protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (!_isInitialized)
                return;
            
            if (stage != CinemachineCore.Stage.Finalize)
            {
                return;
            }


            var offset = _characterCamera.State.RawPosition;
            offset.z = 0;

            state.RawPosition += offset;
            state.Lens.FieldOfView = _characterCamera.State.Lens.FieldOfView;
        }
    }
}