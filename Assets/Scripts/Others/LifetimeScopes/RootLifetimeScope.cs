using Audio;
using Databases;
using DeckEdit.SaveData;
using Others.Scene;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Others.LifetimeScopes
{
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private SpellDatabase spellDatabase;
        [SerializeField] private SpellColorPalette spellColorPalette;
        [SerializeField] private AttackDatabase attackDatabase;
        [SerializeField] private CharacterDatabase characterDatabase;
        [SerializeField] private PlayerConstData playerConstData;

        [SerializeField] private AudioManager audioManager;

        [SerializeField] private SceneFadePanelView sceneFadePanelView;
        [SerializeField] private YesNoDialog yesNoDialog;
        [SerializeField] private OkDialog okDialog;

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("RegisterRoot");


            builder.RegisterInstance(spellDatabase);
            builder.RegisterInstance(spellColorPalette);
            builder.RegisterInstance(attackDatabase);
            builder.RegisterInstance(characterDatabase);
            builder.RegisterInstance(playerConstData);

            builder.Register<DeckSaveDataPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MySceneManager>(Lifetime.Singleton);

            var panelInstance = Instantiate(sceneFadePanelView);
            DontDestroyOnLoad(panelInstance.gameObject);
            builder.RegisterInstance(panelInstance);

            var yesNoInstance = Instantiate(yesNoDialog);
            DontDestroyOnLoad(yesNoInstance.gameObject);
            builder.RegisterInstance(yesNoInstance);


            var okInstance = Instantiate(okDialog);
            DontDestroyOnLoad(okInstance.gameObject);
            builder.RegisterInstance(okInstance);

            Instantiate(audioManager);
        }
    }
}