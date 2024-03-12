using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterRotation
    {
        private const float RotationDuration = 0.2f;

        public bool isRight = true;


        private float _currentAnimatingRot = 90f;

        private readonly Transform _transform;


        public float Rotation
        {
            get => isRight ? 1 : -1;
            set => isRight = value > 0;
        }


        public CharacterRotation(Transform transform)
        {
            _transform = transform;
            _transform.FixedUpdateAsObservable().Subscribe(_ =>
                FixedUpdate()).AddTo(_transform);
        }


        private void FixedUpdate()
        {
            var to = isRight ? 90f : -90f;
            if (Mathf.Abs(to - _currentAnimatingRot) < 0.01f)
                return;

            var lerp = Mathf.Lerp(_currentAnimatingRot, to, RotationDuration);
            _currentAnimatingRot = lerp;
            _transform.eulerAngles = new Vector3(0, _currentAnimatingRot);
        }
    }
}