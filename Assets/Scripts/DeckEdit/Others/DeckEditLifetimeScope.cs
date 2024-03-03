﻿using DeckEdit.Model;
using DeckEdit.View;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Others
{
    public class DeckEditLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CardPool>(Lifetime.Singleton);
            builder.RegisterEntryPoint<DeckList>().AsSelf();
            builder.Register<CurrentSelectedSpell>(Lifetime.Singleton);

            builder.RegisterComponentInHierarchy<CardPoolView>();
            builder.RegisterComponentInHierarchy<DeckListView>();
            builder.RegisterComponentInHierarchy<SaveButtonView>();
            builder.RegisterComponentInHierarchy<DetailView>();

            builder.RegisterEntryPoint<TestInitializer>();
        }
    }
}