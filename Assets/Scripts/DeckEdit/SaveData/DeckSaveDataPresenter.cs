using DeckEdit.Controller;

namespace DeckEdit.SaveData
{
    public class DeckSaveDataPresenter : IDeckSaveDataPresenter
    {
        private const string SaveDataKey = "Deck";

        public DeckData LoadDeck()
        {
            var data = EasySaveWrapper.Load<DeckData>(SaveDataKey);

            if (data != null)
                return data;


            var list = DefaultDeckData.GetDefaultDeck();
            SaveDeck(list);
            return list;
        }

        public void SaveDeck(DeckData deckData)
        {
            EasySaveWrapper.Save(SaveDataKey, deckData);
        }
    }
}