using Audio;
using Battle.Attack;
using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace Battle.Character.Servant
{
    public abstract class ServantBase : CharacterBase
    {
        [Inject] protected readonly PlayerCore playerCore;
        private Vector3 _animatorLocalPosition;

        private int _lifeTest;

        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            _animatorLocalPosition = Animator.transform.localPosition;
        }

        protected UniTask MyDelay(float time)
        {
            return UniTask.Delay(
                (int)(time * 1000f), cancellationToken: destroyCancellationToken);
        }

        protected UniTask TweenToUniTask(Tweener tween)
        {
            return tween.ToUniTask(cancellationToken: destroyCancellationToken);
        }

        public override void OnAttacked(AttackHitController attackHitController)
        {
            var attackData = AttackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
                _lifeTest -= attackData.Damage;


            Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                .OnComplete(() => Animator.transform.localPosition = _animatorLocalPosition);

            AllAudioManager.PlaySe("Hit");
            AttackHitEffectFactory.Create(transform.position, transform.rotation).Forget();
        }

        public ServantBase CreateFromPrefab()
        {
            var instance = Instantiate(this);
            instance.Activate();
            return instance;
        }

        private void Activate()
        {
            
        }

        protected Vector2 GetDirectionToTarget(CharacterBase target)
        {
            return (target.transform.position - transform.position).normalized;
        }
    }
}