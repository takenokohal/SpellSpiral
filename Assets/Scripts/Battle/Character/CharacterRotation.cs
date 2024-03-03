using UnityEngine;

namespace Battle.Character
{
    public class CharacterRotation : MonoBehaviour
    {
        [SerializeField] private float rotationDuration = 0.2f;

        public bool isRight = true;


        private float _currentAnimatingRot = 90f;


        public float Rotation
        {
            get => isRight ? 1 : -1;
            set => isRight = value > 0;
        }


        private void FixedUpdate()
        {
            var to = isRight ? 90f : -90f;
            if (Mathf.Abs(to - _currentAnimatingRot) < 0.01f)
                return;

            var lerp = Mathf.Lerp(_currentAnimatingRot, to, rotationDuration);
            _currentAnimatingRot = lerp;
            transform.eulerAngles = new Vector3(0, _currentAnimatingRot);
        }
    }
}