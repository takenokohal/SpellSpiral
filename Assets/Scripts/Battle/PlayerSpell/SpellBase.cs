using System;
using Audio;
using Battle.Character;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Battle.PlayerSpell
{
    public abstract class SpellBase : SerializedMonoBehaviour
    {
        protected PlayerCore PlayerCore { get; private set; }

        protected PlayerBuff PlayerBuff => PlayerCore.PlayerBuff;

        protected AllCharacterManager AllCharacterManager { get; private set; }

        protected MagicCircleFactory MagicCircleFactory { get; private set; }
        protected SpellDatabase SpellDatabase { get; private set; }
        protected SpellData SpellData { get; private set; }

        protected ServantFactory ServantFactory { get; private set; }

        protected string CharacterKey => PlayerCore.CharacterKey;


        public SpellBase Construct(
            PlayerCore playerCore,
            AllCharacterManager allCharacterManager,
            MagicCircleFactory magicCircleFactory,
            SpellDatabase spellDatabase,
            SpellData spellData,
            ServantFactory servantFactory)
        {
            var instance = Instantiate(this);
            instance.PlayerCore = playerCore;
            instance.AllCharacterManager = allCharacterManager;
            instance.MagicCircleFactory = magicCircleFactory;
            instance.SpellDatabase = spellDatabase;
            instance.SpellData = spellData;
            instance.ServantFactory = servantFactory;
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

        protected Vector2 GetDirectionPlayerToCharacter(CharacterBase target) =>
            (target.transform.position - PlayerCore.transform.position).normalized;
    }
}