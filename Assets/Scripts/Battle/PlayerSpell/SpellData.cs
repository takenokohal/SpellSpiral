using System;
using Databases;
using Others;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Video;

namespace Battle.PlayerSpell
{
    [Serializable]
    public class SpellData
    {
        [SerializeField] private string spellKey;
        [SerializeField] private string name;
        [SerializeField] private int manaCost;

        [SerializeField, GUIColor(nameof(GetColor))]
        private SpellAttribute spellAttribute;

        [SerializeField] private SpellType spellType;
        [SerializeField] private float effectDuration;
        [SerializeField] private string description;

        [SerializeField] private SpellBase spellBase;
        [SerializeField] private Sprite spellIcon;

        [SerializeField] private VideoClip videoClip;


        public bool IsNotImp() => spellBase == null || spellIcon == null;

        public SpellData(string spellKey, string name, int manaCost, SpellAttribute spellAttribute,
            SpellType spellType, float effectDuration, string description, SpellBase spellBase, Sprite spellIcon,
            VideoClip videoClip)
        {
            this.spellKey = spellKey;
            this.name = name;
            this.manaCost = manaCost;
            this.spellAttribute = spellAttribute;
            this.spellType = spellType;
            this.effectDuration = effectDuration;
            this.description = description;
            this.spellBase = spellBase;
            this.spellIcon = spellIcon;
            this.videoClip = videoClip;
        }

        public string SpellKey => spellKey;

        public string SpellName => name;

        public SpellBase SpellBase => spellBase;

        public Sprite SpellIcon => spellIcon;

        public int ManaCost => manaCost;

        public SpellAttribute SpellAttribute => spellAttribute;

        public SpellType SpellType => spellType;

        public float EffectDuration => effectDuration;

        public string SpellDescription => description;

        public VideoClip VideoClip => videoClip;

        private Color GetColor()
        {
            return SpellColorPalette.LoadOnEditor().GetColor(spellAttribute);
        }
    }
}