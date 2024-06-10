using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.Character
{
    public class WizardAnimationController
    {
        private readonly Animator _animator;

        public float HorizontalSpeedValue { get; set; }
        private static readonly int HorizontalAnimKey = Animator.StringToHash("HorizontalSpeed");

        private static bool IsLoopState(AnimationState animationState)
        {
            return animationState is AnimationState.Idle or AnimationState.Charge;
        }

        public WizardAnimationController(Animator animator)
        {
            _animator = animator;

            _animator.FixedUpdateAsObservable().Subscribe(_ =>
            {
                _animator.SetFloat(HorizontalAnimKey, HorizontalSpeedValue);
            }).AddTo(animator);
        }


        public void PlayAnimation(AnimationState animationState)
        {
            if (IsLoopState(animationState))
            {
                _animator.CrossFadeInFixedTime(animationState.ToString(), 0.15f);
            }
            else
            {
                _animator.Play(animationState.ToString(), 0, 0);
            }
        }

        public void SetGraphicVisible(bool visible)
        {
            _animator.gameObject.SetActive(visible);
        }


        public enum AnimationState
        {
            Idle,
            Charge,
            Attack,
            Buff,
            OnDamage
        }
    }
}