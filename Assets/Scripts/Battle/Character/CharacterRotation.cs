using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterRotation
    {
        private const float RotationDuration = 0.2f;

        public bool IsRight { get; private set; }


        private float _currentAnimatingRot = 90f;

        private readonly Transform _transform;

        private const float ToRotation = 120f;

        public float Rotation
        {
            get => IsRight ? 1 : -1;
            set => IsRight = value > 0;
        }


        public CharacterRotation(Transform transform)
        {
            _transform = transform;
            _transform.FixedUpdateAsObservable().Subscribe(_ =>
                FixedUpdate()).AddTo(_transform);
        }


        private void FixedUpdate()
        {
            var to = IsRight ? ToRotation : -ToRotation;
            if (Mathf.Abs(to - _currentAnimatingRot) < 0.01f)
                return;

            var lerp = Mathf.Lerp(_currentAnimatingRot, to, RotationDuration);
            _currentAnimatingRot = lerp;
            _transform.rotation = Quaternion.Euler(0, _currentAnimatingRot, 0);
        }
    }
}