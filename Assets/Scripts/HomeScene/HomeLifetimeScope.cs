using Others.Message;
using VContainer;
using VContainer.Unity;

namespace HomeScene
{
    public class HomeLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.Register<HomeStateController>(Lifetime.Singleton);
            
            builder.RegisterComponentInHierarchy<HomeMenuController>();
            builder.RegisterComponentInHierarchy<MissionSelectController>();

            builder.RegisterEntryPoint<MessageTranslatorInSceneStart>();
        }
    }
}