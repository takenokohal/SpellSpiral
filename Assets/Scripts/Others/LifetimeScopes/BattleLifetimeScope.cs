using Battle;
using Battle.Attack;
using Battle.Character;
using Battle.Character.Player;
using Battle.Character.Player.Deck;
using Battle.Character.Servant;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Battle.CommonObject.Pause;
using Battle.CommonObject.Result;
using Battle.CutIn;
using Battle.MyCamera;
using Battle.PlayerSpell;
using Battle.UI;
using Test;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Others.LifetimeScopes
{
    public class BattleLifetimeScope : LifetimeScope
    {
        //       [SerializeField] private StageObjectHolder stageObjectHolder;

        protected override void Configure(IContainerBuilder builder)
        {
            //    var sceneManager = Parent.Container.Resolve<MySceneManager>();
            //    var stageName = sceneManager.CurrentSceneName;
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

            //    var stageObject = stageObjectHolder.StageObjects[stageName];

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

           // var bgCam = Instantiate(stageObject.BackGroundCameraRoot);
          //  builder.RegisterComponent(bgCam);

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