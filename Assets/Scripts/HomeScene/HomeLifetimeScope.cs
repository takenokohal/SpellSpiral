using VContainer;
using VContainer.Unity;

namespace HomeScene
{
    public class HomeLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<HomeStateChangeController>();
            builder.RegisterComponentInHierarchy<MissionController>();
            builder.RegisterComponentInHierarchy<HomeMenuController>();
        }
    }
}