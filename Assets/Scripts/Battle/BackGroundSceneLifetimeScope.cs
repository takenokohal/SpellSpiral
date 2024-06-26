using VContainer;
using VContainer.Unity;

namespace Battle
{
    public class BackGroundSceneLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<BattleMainSceneLoader>();
        }
    }
}