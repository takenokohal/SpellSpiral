using Battle.PlayerSpell;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace DeckEdit.View.Highlander
{
    public class HighlanderAnimation : MonoBehaviour
    {
        [SerializeField] private Transform centerIcon;

        [SerializeField] private Transform leftIcon;
        [SerializeField] private Transform rightIcon;

        [SerializeField] private Transform leftLeftIcon;
        [SerializeField] private Transform rightRightIcon;

        private Vector3 _leftPos;
        private Vector3 _rightPos;
        private Vector3 _centerPos;

        private float _sideIconScale;
        private float _centerIconScale;

        private Tween _tween;

        private void Start()
        {
            _leftPos = leftIcon.localPosition;
            _rightPos = rightIcon.localPosition;
            _centerPos = centerIcon.localPosition;

            _sideIconScale = leftIcon.localScale.x;
            _centerIconScale = centerIcon.localScale.x;
        }

        public async UniTaskVoid AnimateToRight()
        {
            _tween?.Kill();
            var seq = DOTween.Sequence();
            leftIcon.localPosition = _leftPos * 2;
            centerIcon.localPosition = _leftPos;
            rightIcon.localPosition = _centerPos;
            rightRightIcon.localPosition = _rightPos;

            leftIcon.localScale = Vector3.one * 0;
            centerIcon.localScale = Vector3.one * _sideIconScale;
            rightIcon.localScale = Vector3.one * _centerIconScale;
            rightRightIcon.localScale = Vector3.one * _sideIconScale;

            seq.Join(leftIcon.DOLocalMove(_leftPos, 0.2f));
            seq.Join(centerIcon.DOLocalMove(_centerPos, 0.2f));
            seq.Join(rightIcon.DOLocalMove(_rightPos, 0.2f));
            seq.Join(rightRightIcon.DOLocalMove(_rightPos * 2, 0.2f));

            seq.Join(leftIcon.DOScale(_sideIconScale, 0.2f));
            seq.Join(centerIcon.DOScale(_centerIconScale, 0.2f));
            seq.Join(rightIcon.DOScale(_sideIconScale, 0.2f));
            seq.Join(rightRightIcon.DOScale(0, 0.2f));

            _tween = seq;
        }

        public async UniTaskVoid AnimateToLeft()
        {
            _tween?.Kill();
            var seq = DOTween.Sequence();
            rightIcon.localPosition = _rightPos * 2;
            centerIcon.localPosition = _rightPos;
            leftIcon.localPosition = _centerPos;
            leftLeftIcon.localPosition = _leftPos;

            rightIcon.localScale = Vector3.one * 0;
            centerIcon.localScale = Vector3.one * _sideIconScale;
            leftIcon.localScale = Vector3.one * _centerIconScale;
            leftLeftIcon.localScale = Vector3.one * _sideIconScale;

            seq.Join(leftIcon.DOLocalMove(_leftPos, 0.2f));
            seq.Join(centerIcon.DOLocalMove(_centerPos, 0.2f));
            seq.Join(rightIcon.DOLocalMove(_rightPos, 0.2f));
            seq.Join(leftLeftIcon.DOLocalMove(_leftPos * 2, 0.2f));

            seq.Join(leftIcon.DOScale(_sideIconScale, 0.2f));
            seq.Join(centerIcon.DOScale(_centerIconScale, 0.2f));
            seq.Join(rightIcon.DOScale(_sideIconScale, 0.2f));
            seq.Join(leftLeftIcon.DOScale(0, 0.2f));
            _tween = seq;
        }
    }
}