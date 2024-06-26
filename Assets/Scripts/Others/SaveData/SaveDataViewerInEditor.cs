using System.Collections.Generic;
using System.Linq;
using Battle.PlayerSpell;
using Databases;
using DeckEdit.Model;
using DeckEdit.SaveData;
using Others.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Others.SaveData
{
    [CreateAssetMenu(menuName = "Create SaveDataViewerInEditor", fileName = "SaveDataViewerInEditor", order = 0)]
    public class SaveDataViewerInEditor : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, ValueDropdown(nameof(GetStrings)), ListDrawerSettings(ShowPaging = false)]
        private List<string> currentSavedDeck;

        [SerializeField, ValueDropdown(nameof(GetHighlanderStrings))]
        private string highlanderSpell;


        [Button]
        private void Load()
        {
            var v = new DeckSaveDataPresenter();
            var deck = v.LoadDeck();
            currentSavedDeck = deck.normalSpellDeck;
            highlanderSpell = deck.highlanderSpell;
        }

        [Button]
        private void Save()
        {
            var v = new DeckSaveDataPresenter();
            v.SaveDeck(new DeckData(currentSavedDeck, highlanderSpell));
        }

        [Button]
        private void SetAllSpell(string str)
        {
            var list = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(str);
            }

            currentSavedDeck = list;
        }

        [Button]
        private void SetRandom()
        {
            var db = SpellDatabase.LoadOnEditor();
            var keys = db.SpellDictionary.Where(value => !value.Value.IsNotImp()).Select(value => value.Key);

            var list = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(keys.GetRandomValue());
            }

            currentSavedDeck = list;
        }

        private IEnumerable<string> GetStrings()
        {
            var v = SpellDatabase.LoadOnEditor();
            return v.SpellDictionary.Where(value => value.Value.SpellType != SpellType.Highlander)
                .Select(value => value.Key);
        }

        private IEnumerable<string> GetHighlanderStrings()
        {
            var v = SpellDatabase.LoadOnEditor();
            return v.SpellDictionary.Where(value => value.Value.SpellType == SpellType.Highlander)
                .Select(value => value.Key);
        }


        [Button]
        private void SetAllTypeSpell()
        {
            var db = SpellDatabase.LoadOnEditor();
            var keys = db.SpellDictionary.Where(value => !value.Value.IsNotImp()).Select(value => value.Key);

            currentSavedDeck = keys.ToList();
        }

        [Button]
        private void SetDefault()
        {
            var defaultDeck = DefaultDeckData.GetDefaultDeck();
            currentSavedDeck = defaultDeck.normalSpellDeck;
            highlanderSpell = defaultDeck.highlanderSpell;

            var db = SpellDatabase.LoadOnEditor();
        }
#endif
    }
}