using Battle.Attack;
using Battle.Character.Player;
using Battle.MyCamera;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace Battle.Character.Enemy
{
    public abstract class EnemyBase : CharacterBase
    {
        [Inject] protected readonly PlayerCore playerCore;
        [Inject] protected readonly SpecialCameraSwitcher specialCameraSwitcher;


        private Vector3 _animatorLocalPosition;

        public Vector2 GetDirectionToPlayer()
        {
            return playerCore.GetDirectionToPlayer(transform.position);
        }


        public EnemyParameter EnemyParameter { get; private set; }


        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            EnemyParameter = new EnemyParameter(characterDatabase.Find(CharacterKey).Life);

            _animatorLocalPosition = Animator.transform.localPosition;
            this.OnDestroyAsObservable().Subscribe(_ => allEnemyManager.RemoveEnemy(this)).AddTo(this);
        }


        public override void OnAttacked(AttackHitController attackHitController)
        {
            var attackData = attackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
                EnemyParameter.CurrentLife -= attackData.Damage;

            characterCamera.ImpulseSource.GenerateImpulse(5);


            Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                .OnComplete(() => Animator.transform.localPosition = _animatorLocalPosition);
            attackHitEffectFactory.Create(transform.position, transform.rotation);
        }

        public override OwnerType GetOwnerType()
        {
            return OwnerType.Enemy;
        }

        public void LookPlayer()
        {
            CharacterRotation.Rotation = GetDirectionToPlayer().x;
        }
    }
}