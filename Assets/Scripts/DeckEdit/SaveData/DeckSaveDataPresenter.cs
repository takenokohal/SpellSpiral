using System.Collections.Generic;
using DeckEdit.Model;
using UnityEngine;

namespace DeckEdit.SaveData
{
    public class DeckSaveDataPresenter : IDeckSaveDataPresenter
    {
        private const string SaveDataKey = "Deck";

        public List<string> LoadDeck()
        {
            var data = EasySaveWrapper.Load<DeckSaveData>(SaveDataKey);

            if (data != null)
                return data.deckData;


            var list = DefaultDeckData.GetDefaultDeck();
            SaveDeck(list);
            return list;
        }

        public void SaveDeck(List<string> deck)
        {
            EasySaveWrapper.Save(SaveDataKey, new DeckSaveData()
            {
                deckData = deck
            });
        }
    }
}