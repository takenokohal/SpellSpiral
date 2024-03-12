using System;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Battle.PlayerSpell
{
    public abstract class SpellBase : MonoBehaviour
    {
        protected PlayerCore PlayerCore { get; private set; }

        protected AllEnemyManager AllEnemyManager { get; private set; }

        protected MagicCircleFactory MagicCircleFactory { get; private set; }

        protected PlayerBuff PlayerBuff { get; private set; }

        protected SpellData SpellData { get; private set; }


        public SpellBase Construct(
            PlayerCore playerCore,
            AllEnemyManager allEnemyManager,
            MagicCircleFactory magicCircleFactory,
            PlayerBuff playerBuff, SpellData spellData)
        {
            var instance = Instantiate(this);
            instance.PlayerCore = playerCore;
            instance.AllEnemyManager = allEnemyManager;
            instance.MagicCircleFactory = magicCircleFactory;
            instance.PlayerBuff = playerBuff;
            instance.SpellData = spellData;
            instance.Init();
            return instance;
        }

        protected abstract UniTaskVoid Init();

        private readonly Subject<Unit> _onDead = new();
        public IObservable<Unit> OnDead => _onDead;

        protected void Kill()
        {
            _onDead.OnNext(Unit.Default);
            Destroy(gameObject);
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

        protected Vector2 GetDirectionToEnemy(EnemyBase target) =>
            (target.Center.position - PlayerCore.Center.position).normalized;
    }
}