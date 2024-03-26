using Cinemachine;
using UnityEngine;

namespace Battle.MyCamera
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        public CinemachineVirtualCamera VirtualCamera => virtualCamera;

        [SerializeField] private CinemachineImpulseSource impulseSource;

        public CinemachineImpulseSource ImpulseSource => impulseSource;
    }
}