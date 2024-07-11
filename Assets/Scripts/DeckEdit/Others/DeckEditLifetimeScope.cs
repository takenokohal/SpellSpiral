using DeckEdit.Controller;
using DeckEdit.Model;
using DeckEdit.SaveData;
using DeckEdit.View;
using DeckEdit.View.CardPool;
using DeckEdit.View.Highlander;
using DeckEdit.View.MyDeck;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Others
{
    public class DeckEditLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            //Controller
            builder.RegisterEntryPoint<DeckEditInitializer>();
            builder.RegisterEntryPoint<MyDeckController>();
            builder.RegisterEntryPoint<CardPoolController>();
            //    builder.RegisterEntryPoint<StateController>();
            builder.RegisterComponentInHierarchy<DeckEditActiveController>();

            //Model
            builder.Register<MyDeckModel>(Lifetime.Singleton);
            builder.Register<CurrentSelectedSpell>(Lifetime.Singleton);
            builder.Register<DeckEditStateModel>(Lifetime.Singleton);
            builder.Register<CardPoolModel>(Lifetime.Singleton);

            //View
            builder.RegisterComponentInHierarchy<MyDeckListView>();
            builder.RegisterComponentInHierarchy<MyDeckCursorView>();
            builder.RegisterComponentInHierarchy<CardPoolCursorView>();
            builder.RegisterComponentInHierarchy<CardPoolListView>();
            builder.RegisterComponentInHierarchy<CardPoolScrollView>();
            builder.RegisterComponentInHierarchy<HighlanderViewAndController>();
            builder.RegisterComponentInHierarchy<HighlanderAnimation>();

            builder.RegisterComponentInHierarchy<DetailView>();
        }
    }
}