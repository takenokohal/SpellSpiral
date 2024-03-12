using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Character.Enemy.Servant
{
    public abstract class ServantBase : SerializedMonoBehaviour
    {
        protected PlayerCore PlayerCore { get; private set; }
        
        protected EnemyBase EnemyBase { get; private set; }

        public void Init(PlayerCore playerCore)
        {
            PlayerCore = playerCore;
            EnemyBase = GetComponent<EnemyBase>();
        }
        
        protected Vector2 GetDirectionToPlayer() => EnemyBase.GetDirectionToPlayer();

        protected UniTask MyDelay(float time)
        {
            return UniTask.Delay(
                (int)(time * 1000f), cancellationToken: destroyCancellationToken);
        }

        protected UniTask TweenToUniTask(Tweener tween)
        {
            return tween.ToUniTask(cancellationToken: destroyCancellationToken);
        }
    }
}