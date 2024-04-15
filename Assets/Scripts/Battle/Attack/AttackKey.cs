using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Attack
{
    [Serializable]
    public class AttackKey
    {
        [SerializeField, ValueDropdown(nameof(GetCharacterKeys))]
        private string characterKey;

        [SerializeField, ValueDropdown(nameof(GetDetailKeys))]
        private string detailKey;

        public string CharacterKey => characterKey;

        public string DetailKey => detailKey;

        public static IEnumerable<string> GetCharacterKeys()
        {
#if UNITY_EDITOR
            var db = AttackDatabase.LoadOnEditor();
            return db.AttackDataDictionary.Select(value => value.Key);

#else
            return null;
#endif
        }

        private IEnumerable<string> GetDetailKeys()
        {
#if UNITY_EDITOR
            var db = AttackDatabase.LoadOnEditor();
            return db.AttackDataDictionary[characterKey].Select(value => value.Key);
#else
            return null;
#endif
        }
    }
}