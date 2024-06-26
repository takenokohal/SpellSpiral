using Battle.Attack;
using Battle.Character.Player.Buff;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Battle.Character.Servant
{
    public class ServantBase : CharacterBase
    {
        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            OnDeadObservable().Subscribe(_ => DeadAnimation().Forget());
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

        private async UniTaskVoid DeadAnimation()
        {
            AllCharacterManager.RemoveCharacter(this);

            TweenToUniTask(transform.DOShakePosition(1, 0.2f, 20, fadeOut: false)).Forget();
            await TweenToUniTask(transform.DOScale(2, 1));
            AttackHitEffectFactory.CreateDeadEffect(transform.position, transform.rotation, 1).Forget();
            Destroy(gameObject);
        }

        protected Vector2 GetDirectionToTarget(CharacterBase target)
        {
            return (target.transform.position - transform.position).normalized;
        }

        protected void LookAtTarget(CharacterBase target)
        {
            CharacterRotation.Rotation = GetDirectionToTarget(target).x;
        }

        protected override float CalcDamage(AttackHitController attackHitController)
        {
            var damage = AttackDatabase.Find(attackHitController.AttackKey).Damage;

            if (GetOwnerType() == OwnerType.Player)
                return damage;

            var playerCore = AllCharacterManager.PlayerCore;
            var attackBuff = playerCore.PlayerBuff.BuffCount(BuffKey.BuffMultiply);
            for (int i = 0; i < attackBuff; i++)
            {
                damage *= playerCore.PlayerConstData.BuffPowerRatio;
            }

            return damage;
        }
    }
}