using DeckEdit.Model;
using VContainer;
using VContainer.Unity;

namespace DeckEdit.Controller
{
    public class DeckEditInitializer : IInitializable
    {
      //  [Inject] private readonly CardPool _cardPool;
        [Inject] private readonly MyDeckModel _myDeckModel;

        [Inject] private readonly IDeckSaveDataPresenter _deckSaveDataPresenter;
        public void Initialize()
        {
    //        _cardPool.Init();
            
            InitMyDeck();
        }

        private void InitMyDeck()
        {
            _myDeckModel.SetDeckData(_deckSaveDataPresenter.LoadDeck());
        }
    }
}