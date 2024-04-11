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
            return data == null ? new List<string>() : data.deckData;
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