using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using VContainer;

namespace Battle.Character.Servant
{
    public abstract class ServantBase : CharacterBase
    {
        [Inject] protected readonly PlayerCore playerCore;


        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            OnDeadObservable().Subscribe(_ => Dead());
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


        public ServantBase CreateFromPrefab()
        {
            var instance = Instantiate(this);
            instance.Activate();
            return instance;
        }

        private void Activate()
        {
        }

        private void Dead()
        {
            AllCharacterManager.RemoveCharacter(this);
            Destroy(gameObject);
        }

        protected Vector2 GetDirectionToTarget(CharacterBase target)
        {
            return (target.transform.position - transform.position).normalized;
        }
    }
}