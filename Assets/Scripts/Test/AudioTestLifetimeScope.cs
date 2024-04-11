using VContainer;
using VContainer.Unity;

namespace Test
{
    public class AudioTestLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<AudioTest>();
        }
    }
}