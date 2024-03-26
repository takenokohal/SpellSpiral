using VContainer;
using VContainer.Unity;

namespace Others
{
    public class BackGroundSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BattleMainSceneLoader>();
        }
    }
}