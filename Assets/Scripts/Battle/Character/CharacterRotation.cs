using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.Character
{
    public class CharacterRotation
    {
        private const float RotationDuration = 0.2f;
        private const float ToRotation = 120f;


        public bool IsRight { get; set; }


        private float _currentAnimatingRot;

        private readonly Transform _transform;


        public bool IsStop { get; set; }

        public float Rotation
        {
            get => IsRight ? 1 : -1;
            set => IsRight = value > 0;
        }


        public CharacterRotation(Transform transform)
        {
            _transform = transform;
            _currentAnimatingRot = _transform.eulerAngles.y;
            _transform.FixedUpdateAsObservable().Subscribe(_ =>
                FixedUpdate()).AddTo(_transform);
        }


        private void FixedUpdate()
        {
            if (IsStop)
                return;
            var to = IsRight ? ToRotation : -ToRotation;
            if (Mathf.Abs(to - _currentAnimatingRot) < 0.01f)
                return;

            var lerp = Mathf.Lerp(_currentAnimatingRot, to, RotationDuration);
            _currentAnimatingRot = lerp;
            _transform.rotation = Quaternion.Euler(0, _currentAnimatingRot, 0);
        }
    }
}