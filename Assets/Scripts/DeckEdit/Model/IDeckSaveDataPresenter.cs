using System.Collections.Generic;

namespace DeckEdit.Model
{
    public interface IDeckSaveDataPresenter
    {
        public DeckData LoadDeck();

        public void SaveDeck(DeckData deckData);
    }
}