using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.Character.Player.Deck;
using Battle.CommonObject.MagicCircle;
using Battle.CommonObject.Pause;
using Battle.CommonObject.Result;
using Battle.MyCamera;
using Battle.PlayerSpell;
using Battle.UI;
using Cysharp.Threading.Tasks;
using Databases;
using Others.Scene;
using Test;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Others.LifetimeScopes
{
    public class BattleLifetimeScope : LifetimeScope
    {
        [SerializeField] private StageObjectHolder stageObjectHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            var sceneManager = Parent.Container.Resolve<MySceneManager>();
            var stageName = sceneManager.CurrentSceneName;
            /*
            if (sceneManager.CurrentSceneName == "BattleMain")
            {
                UniTask.Void(async () =>
                {
                    await UniTask.Yield();
                    sceneManager.ChangeSceneAsync("Training").Forget();
                });
            }
            */

            var stageObject = stageObjectHolder.StageObjects[stageName];

            builder.RegisterEntryPoint<GameLoop>().AsSelf();


            //Enemy
            builder.Register<AllEnemyManager>(Lifetime.Singleton);

            var enemy = Instantiate(stageObject.Enemy);
            builder.RegisterComponent(enemy);

            //Player
            builder.RegisterComponentInHierarchy<PlayerCore>();
            builder.RegisterComponentInHierarchy<PlayerMover>();
            builder.RegisterComponentInHierarchy<PlayerChant>();
            builder.RegisterComponentInHierarchy<PlayerBuff>();
            builder.RegisterComponentInHierarchy<PlayerCharge>();

            //Camera
            builder.RegisterComponentInHierarchy<SpecialCameraSwitcher>();
            builder.RegisterComponentInHierarchy<CharacterCamera>();

            var bgCam = Instantiate(stageObject.BackGroundCameraRoot);
            builder.RegisterComponent(bgCam);
            //Factory
            builder.Register<SpellFactory>(Lifetime.Singleton);
            builder.RegisterComponentInHierarchy<AttackHitEffectFactory>();
            builder.RegisterComponentInHierarchy<MagicCircleFactory>();

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

            //Intro
            builder.RegisterComponentInHierarchy<IntroController>();
        }
    }
}