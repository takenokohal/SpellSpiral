﻿using Audio;
using Config;
using Databases;
using DeckEdit.Controller;
using DeckEdit.Model;
using DeckEdit.SaveData;
using DeckEdit.View;
using DeckEdit.View.CardPool;
using DeckEdit.View.Highlander;
using DeckEdit.View.MyDeck;
using Others.Dialog;
using Others.Input;
using Others.Message;
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
        [SerializeField] private MessageDatabase messageDatabase;


        [SerializeField] private SeManager seManager;

        [SerializeField] private SceneFadePanelView sceneFadePanelView;
        [SerializeField] private YesNoDialog yesNoDialog;
        [SerializeField] private OkDialog okDialog;
        [SerializeField] private MyInputManager myInputManager;


        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("RegisterRoot");

            // Screen.SetResolution(1920, 1080, FullScreenMode.FullScreenWindow);

            builder.RegisterInstance(spellDatabase);
            builder.RegisterInstance(spellColorPalette);
            builder.RegisterInstance(attackDatabase);
            builder.RegisterInstance(characterDatabase);
            builder.RegisterInstance(playerConstData);
            builder.RegisterInstance(messageDatabase);

            builder.Register<DeckSaveDataPresenter>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<MySceneManager>(Lifetime.Singleton);
            builder.Register<MessageManager>(Lifetime.Singleton);

            builder.Register<ConfigController>(Lifetime.Singleton);

            builder.RegisterEntryPoint<AllAudioManager>();

            InstantiateDontDestroyRegister(sceneFadePanelView, builder);
            InstantiateDontDestroyRegister(yesNoDialog, builder);
            InstantiateDontDestroyRegister(okDialog, builder);

            InstantiateDontDestroyRegister(seManager, builder);
            InstantiateDontDestroyRegister(myInputManager, builder);
        }

        private void InstantiateDontDestroyRegister<T>(T prefab, IContainerBuilder builder) where T : MonoBehaviour
        {
            var instance = Instantiate(prefab);
            DontDestroyOnLoad(instance.gameObject);
            builder.RegisterComponent(instance);
        }
    }
}