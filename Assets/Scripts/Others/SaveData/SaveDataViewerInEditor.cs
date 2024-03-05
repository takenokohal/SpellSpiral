using System.Collections.Generic;
using System.Linq;
using Databases;
using DeckEdit.SaveData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Others.SaveData
{
    [CreateAssetMenu(menuName = "Create SaveDataViewerInEditor", fileName = "SaveDataViewerInEditor", order = 0)]
    public class SaveDataViewerInEditor : SerializedScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, ValueDropdown(nameof(GetStrings)), ListDrawerSettings(ShowPaging = false)] private List<string> currentSavedDeck;

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


        private IEnumerable<string> GetStrings()
        {
            var v = SpellDatabase.LoadOnEditor();
            return v.SpellDictionary.Select(value => value.Key);
        }
#endif
    }
}