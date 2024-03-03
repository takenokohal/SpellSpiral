using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.Character.Player.Deck;
using Battle.CommonObject.MagicCircle;
using Battle.CommonObject.Result;
using Battle.MyCamera;
using Battle.PlayerSpell;
using Cinemachine;
using Test;
using VContainer;
using VContainer.Unity;

namespace Others.LifetimeScopes
{
    public class BattleLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameLoop>().AsSelf();


            builder.Register<SpellFactory>(Lifetime.Singleton);
            builder.Register<AllEnemyManager>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<PlayerCore>();
            builder.RegisterComponentInHierarchy<PlayerChant>();
            builder.RegisterComponentInHierarchy<PlayerBuff>();
            builder.RegisterComponentInHierarchy<SpecialCameraSwitcher>();

            builder.RegisterComponentInHierarchy<CinemachineImpulseSource>();

            builder.RegisterComponentInHierarchy<AttackHitEffectFactory>();

            builder.RegisterComponentInHierarchy<MagicCircleFactory>();

            builder.RegisterComponentInHierarchy<LoseController>();
            builder.RegisterComponentInHierarchy<WinController>();

            builder.Register<BattleDeck>(Lifetime.Singleton);
            builder.Register<TestDeckPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

          //  builder.RegisterComponentInHierarchy<SEController>();
        }
    }
}