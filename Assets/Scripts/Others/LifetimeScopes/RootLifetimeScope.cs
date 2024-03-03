using Audio;
using Databases;
using DeckEdit.SaveData;
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

        [SerializeField] private SceneChanger sceneChanger;
        [SerializeField] private AudioManager audioManager;
        
        
        protected override void Configure(IContainerBuilder builder)
        {

            Debug.Log("RegisterRoot");
            
            
            builder.RegisterInstance(spellDatabase);
            builder.RegisterInstance(spellColorPalette);
            builder.RegisterInstance(attackDatabase);
            builder.RegisterInstance(characterDatabase);
            builder.RegisterInstance(playerConstData);

            builder.Register<DeckSaveDataPresenter>(Lifetime.Singleton).AsImplementedInterfaces();

            Instantiate(sceneChanger);
            Instantiate(audioManager);
        }
    }
}