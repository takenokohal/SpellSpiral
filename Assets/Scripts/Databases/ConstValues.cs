using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create ConstValues", fileName = "ConstValues", order = 0)]
    public class ConstValues : ScriptableObject
    {
        [SerializeField] private float cameraImpulse;

        public float CameraImpulse => cameraImpulse;
    }
}