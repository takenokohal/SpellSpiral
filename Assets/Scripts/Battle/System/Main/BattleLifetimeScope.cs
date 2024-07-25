using Battle.Attack;
using Battle.Character;
using Battle.Character.Player;
using Battle.Character.Player.Deck;
using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
using Battle.System.CutIn;
using Battle.System.Pause;
using Battle.System.Result;
using Battle.UI;
using Spell;
using Test;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Battle.System.Main
{
    public class BattleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {

            Debug.Log("Register Battle");

            //Initialize
            builder.RegisterComponentInHierarchy<BattleInitializer>();
            builder.Register<BattleLoop>(Lifetime.Singleton);

            //Character
            builder.Register<CharacterFactory>(Lifetime.Singleton);
            builder.Register<AllCharacterManager>(Lifetime.Singleton);

            //Player
            builder.RegisterComponentInHierarchy<PlayerCore>();
            builder.RegisterComponentInHierarchy<PlayerMover>();
            builder.RegisterComponentInHierarchy<PlayerChant>();
            builder.RegisterComponentInHierarchy<PlayerCharge>();

            //Camera
            builder.RegisterComponentInHierarchy<CameraSwitcher>();
            builder.RegisterComponentInHierarchy<CharacterCamera>();


            //Factory
            builder.Register<SpellFactory>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<AttackHitEffectFactory>();
            builder.RegisterComponentInHierarchy<MagicCircleFactory>();
            builder.RegisterComponentInHierarchy<ReadyEffectFactory>();

            //Result
            builder.RegisterComponentInHierarchy<LoseController>();
            builder.RegisterComponentInHierarchy<LoseMenu>();
            builder.RegisterComponentInHierarchy<WinController>();


            //Deck
            builder.Register<BattleDeck>(Lifetime.Singleton);
            builder.Register<DeckPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

            //Pause
            builder.RegisterComponentInHierarchy<PauseController>();

            //UI
            builder.RegisterComponentInHierarchy<SpellIconView>();
            builder.RegisterComponentInHierarchy<PlayerLifeView>();
            builder.RegisterComponentInHierarchy<PlayerManaView>();
            builder.RegisterComponentInHierarchy<BossLifeView>();
            builder.RegisterComponentInHierarchy<PlayerBuffView>();

            //CutIn
            builder.RegisterComponentInHierarchy<CutInController>();

            //Intro
            builder.RegisterComponentInHierarchy<IntroController>();
        }
    }
}