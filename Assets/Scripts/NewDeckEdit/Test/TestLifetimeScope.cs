using VContainer;
using VContainer.Unity;

namespace NewDeckEdit.Test
{
    public class TestLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            base.Configure(builder);

            builder.RegisterComponentInHierarchy<CardPoolTest>();
        }
    }
}