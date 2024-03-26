using UnityEngine;
using VContainer;

namespace Battle.MyCamera
{
    public class BackGroundCameraRoot : MonoBehaviour
    {
        [SerializeField] private BackGroundCamera  backGroundCamera;

        [Inject] private readonly CharacterCamera _characterCamera;

        private void Start()
        {
            backGroundCamera.Initialize(_characterCamera.VirtualCamera);
        }
    }
}