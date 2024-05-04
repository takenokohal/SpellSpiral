using System.Collections.Generic;
using System.Linq;
using Databases;
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

        [Button]
        private void Load()
        {
            var v = new DeckSaveDataPresenter();
            currentSavedDeck = v.LoadDeck();
        }

        [Button]
        private void Save()
        {
            var v = new DeckSaveDataPresenter();
            v.SaveDeck(currentSavedDeck);
        }

        [Button]
        private void SetAllSpell(string str)
        {
            var list = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                list.Add(str);
            }

            var v = new DeckSaveDataPresenter();
            v.SaveDeck(list);
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
            return v.SpellDictionary.Select(value => value.Key);
        }

        [Button]
        private void SetAllTypeSpell()
        {
            var db = SpellDatabase.LoadOnEditor();
            var keys = db.SpellDictionary.Where(value => !value.Value.IsNotImp()).Select(value => value.Key);

            currentSavedDeck = keys.ToList();
        }
#endif
    }
}