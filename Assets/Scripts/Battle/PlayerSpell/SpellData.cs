using System;
using System.Collections.Generic;
using Databases;
using Others;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.Video;

namespace Battle.PlayerSpell
{
    [Serializable]
    public class SpellData
    {
        [SerializeField] private string spellKey;

        [OdinSerialize] private Dictionary<SystemLanguage, string> _names;
        public string GetName(SystemLanguage language) => _names[language];
        [SerializeField] private int manaCost;

        [SerializeField, GUIColor(nameof(GetColor))]
        private SpellAttribute spellAttribute;

        [SerializeField] private SpellType spellType;
        [SerializeField] private float effectDuration;
        [OdinSerialize] private Dictionary<SystemLanguage, string> _descriptions;
        public string GetDescription(SystemLanguage systemLanguage) => _descriptions[systemLanguage];

        [SerializeField] private SpellBase spellBase;
        [SerializeField] private Sprite spellIcon;


        public bool IsNotImp() => spellBase == null || spellIcon == null;

        public SpellData(string spellKey, Dictionary<SystemLanguage, string> names, int manaCost,
            SpellAttribute spellAttribute, SpellType spellType, float effectDuration,
            Dictionary<SystemLanguage, string> descriptions, SpellBase spellBase, Sprite spellIcon)
        {
            this.spellKey = spellKey;
            _names = names;
            this.manaCost = manaCost;
            this.spellAttribute = spellAttribute;
            this.spellType = spellType;
            this.effectDuration = effectDuration;
            _descriptions = descriptions;
            this.spellBase = spellBase;
            this.spellIcon = spellIcon;
        }

        public string SpellKey => spellKey;
        
        public SpellBase SpellBase => spellBase;

        public Sprite SpellIcon => spellIcon;

        public int ManaCost => manaCost;

        public SpellAttribute SpellAttribute => spellAttribute;

        public SpellType SpellType => spellType;

        public float EffectDuration => effectDuration;
        
        private Color GetColor()
        {
            return SpellColorPalette.LoadOnEditor().GetColor(spellAttribute);
        }
    }
}