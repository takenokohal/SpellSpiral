using System.Collections.Generic;
using Others;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Spell;
using UnityEditor;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create SpellColorPalette", fileName = "SpellColorPalette", order = 0)]
    public class SpellColorPalette : SerializedScriptableObject
    {
        public static SpellColorPalette LoadOnEditor()
        {
#if UNITY_EDITOR
            const string path = "Assets/Databases/SpellColorPalette.asset";
            return AssetDatabase.LoadAssetAtPath<SpellColorPalette>(path);
#else
 return null;
#endif
        }

        [OdinSerialize] private readonly Dictionary<SpellAttribute, Color> _palette = new();
        public Color GetColor(SpellAttribute attribute) => _palette[attribute];
    }
}