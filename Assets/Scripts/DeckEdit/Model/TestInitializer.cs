using VContainer;
using VContainer.Unity;

namespace DeckEdit.Model
{
    public class TestInitializer : IInitializable
    {
        [Inject] private readonly CardPool _cardPool;
        public void Initialize()
        {
            _cardPool.Init();
        }
    }
}