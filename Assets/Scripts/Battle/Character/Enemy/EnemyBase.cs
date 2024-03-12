using Battle.Attack;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.Character.Enemy
{
    public abstract class EnemyBase : CharacterBase
    {

        private CinemachineImpulseSource _impulseSource;


        public Vector2 GetDirectionToPlayer()
        {
            return playerCore.GetDirectionToPlayer(transform.position);
        }


        public EnemyParameter EnemyParameter { get; private set; }


        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            _impulseSource = FindObjectOfType<CinemachineImpulseSource>();
            EnemyParameter = new EnemyParameter(characterDatabase.Find(CharacterKey).Life);

            this.OnDestroyAsObservable().Subscribe(_ => allEnemyManager.RemoveEnemy(this)).AddTo(this);
        }


        public override void OnAttacked(AttackHitController attackHitController)
        {
            var attackData = attackDatabase.Find(attackHitController.AttackKey);
            if (attackData != null)
                EnemyParameter.CurrentLife -= attackData.Damage;

            _impulseSource.GenerateImpulse(5);


            Animator.transform.DOShakePosition(0.1f, 0.1f, 2)
                .OnComplete(() => Animator.transform.localPosition = Vector3.zero);
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