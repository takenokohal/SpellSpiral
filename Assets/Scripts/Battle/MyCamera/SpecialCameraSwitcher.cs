using Cinemachine;
using UnityEngine;

namespace Battle.MyCamera
{
    public class SpecialCameraSwitcher : MonoBehaviour
    {
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private void Start()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void SetSwitch(bool isOn)
        {
            _cinemachineVirtualCamera.enabled = isOn;
        }
    }
}