using Battle.Character;
using Battle.Character.Enemy;
using Cinemachine;
using Others;
using UnityEngine;
using VContainer;

namespace Battle.MyCamera
{
    public class CameraSwitcher : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour specialCamera;

        [SerializeField] private MonoBehaviour cutInCamera;

        [SerializeField] private CinemachineBrain brain;

        [Inject] private readonly AllCharacterManager _allCharacterManager;

        public void SetSpecialCameraSwitch(bool isOn)
        {
            specialCamera.enabled = isOn;
        }

        public void SetCutInSwitch(bool isOn)
        {
            var playerCoreTransform = _allCharacterManager.PlayerCore.transform;
            var parent = cutInCamera.transform.parent;
            parent.position = playerCoreTransform.position;
            cutInCamera.enabled = isOn;
        }

        public void SetIgnoreTimeScale(bool isOn)
        {
            brain.m_IgnoreTimeScale = isOn;
        }
    }
}