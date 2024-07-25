using Cinemachine;
using Databases;
using UnityEngine;
using VContainer;

namespace Battle.MyCamera
{
    public class CharacterCamera : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera virtualCamera;

        public CinemachineVirtualCamera VirtualCamera => virtualCamera;

        [SerializeField] private CinemachineImpulseSource impulseSource;

        public CinemachineImpulseSource ImpulseSource => impulseSource;

        [Inject] private readonly ConstValues _constValues;

        public void Impulse(float damage)
        {
            var force = 1f + damage / 50f;
            ImpulseSource.GenerateImpulse(force);
        }
    }
}