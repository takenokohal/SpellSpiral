using System.Collections.Generic;
using System.Linq;
using Battle.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create CharacterDatabase", fileName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : SerializedScriptableObject
    {
        [SerializeField] private List<CharacterData> characterDataList;

        public IReadOnlyList<CharacterData> CharacterDataList => characterDataList;

        public CharacterData Find(string characterKey) =>
            CharacterDataList.First(value => value.CharacterKey == characterKey);
    }
}