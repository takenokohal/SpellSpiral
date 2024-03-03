using System;
using System.Collections.Generic;
using Others;
using Others.Utils;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Battle.Character
{
    [Serializable]
    public class CharacterData
    {
        [SerializeField, ValueDropdown(nameof(GetCharacterKey))]
        private string characterKey;

        public CharacterKey CharacterKey => Enum.Parse<CharacterKey>(characterKey);

        [SerializeField] private int life;

        public int Life => life;

        //EditorSetting
        private IEnumerable<string> GetCharacterKey() => EnumUtil<CharacterKey>.GetStrings();

        [SerializeField] private Sprite magicCircleSprite;
        public Sprite MagicCircleSprite => magicCircleSprite;

        public float ChantTime => CharacterKey == CharacterKey.Player ? 0.2f : 1f;
    }
}