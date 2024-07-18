using Config;
using Databases;
using UnityEngine;
using VContainer;

namespace Others.Message
{
    public class MessageManager
    {
        [Inject] private readonly MessageDatabase _messageDatabase;
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly ConfigController _configController;

        public bool IsInitialized => _configController.DataIsLoaded;


        public string GetMessage(string key)
        {
            var data = _messageDatabase.Find(key);
            return data.GetText(GetLanguage());
        }

        public string GetCharacterName(string key)
        {
            var data = _characterDatabase.Find(key);
            return data.CharacterNames[GetLanguage()];
        }

        public string GetSpellName(string key)
        {
            var data = _spellDatabase.Find(key);
            return data.GetName(GetLanguage());
        }

        public string GetSpellDescription(string key)
        {
            var data = _spellDatabase.Find(key);
            return data.GetDescription(GetLanguage());
        }

        private SystemLanguage GetLanguage()
        {
            var saveData = _configController.CurrentConfigData;
            return saveData.language;
        }
    }
}