using System.Collections.Generic;

namespace DeckEdit.Model
{
    public interface IDeckSaveDataPresenter
    {
        public List<string> LoadDeck();
        public void SaveDeck(List<string> deck);
    }
}