using DeckEdit.Model;

namespace DeckEdit.Controller
{
    public interface IDeckSaveDataPresenter
    {
        public DeckData LoadDeck();

        public void SaveDeck(DeckData deckData);
    }
}