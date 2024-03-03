using System;
using System.Collections.Generic;
using System.Linq;
using Battle.PlayerSpell;
using Cysharp.Threading.Tasks;
using Others;
using Others.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create SpellDatabase", fileName = "SpellDatabase", order = 0)]
    public class SpellDatabase : SerializedScriptableObject
    {
        [OdinSerialize, Searchable] private readonly Dictionary<string, SpellData> _spellDictionary = new();
        public IReadOnlyDictionary<string, SpellData> SpellDictionary => _spellDictionary;

        public SpellData Find(string key) => SpellDictionary[key];


        public static SpellDatabase LoadOnEditor()
        {
#if UNITY_EDITOR
            var v = AssetDatabase.LoadAssetAtPath<SpellDatabase>(PathsAndURL.CreateDatabasePath<SpellDatabase>());
            return v;

#else
            return null;
#endif
        }

#if UNITY_EDITOR

        [Button]
        private void Update()
        {
            UpdateAsync().Forget();
        }

        private async UniTaskVoid UpdateAsync()
        {
            Debug.Log("UpdateStart");
            var req = UnityWebRequest.Get(PathsAndURL.SpellDatabaseSpreadSheetURL);
            await req.SendWebRequest();

            if (req.result == UnityWebRequest.Result.Success)
            {
                var text = req.downloadHandler.text;
                Parse(text);
                Debug.Log("UpdateComplete");
            }
            else
            {
                Debug.LogError(req.error);
            }

            req.Dispose();
        }

        private void Parse(string csv)
        {
            _spellDictionary.Clear();

            var rows = csv.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var row in rows.Skip(1))
            {
                var cells = row.Split(new[] { ',' });

                var spellKey = cells[0].Trim('"');
                var spellName = cells[1].Trim('"');
                var cost = int.TryParse(cells[2].Trim('"'), out var costResult) ? costResult : 0;
                var attribute = Enum.TryParse<SpellAttribute>(cells[3].Trim('"'), out var attributeResult)
                    ? attributeResult
                    : SpellAttribute.Fire;

                var spellType = Enum.TryParse<SpellType>(cells[4].Trim('"'), out var typeResult)
                    ? typeResult
                    : SpellType.Attack;

                var duration = float.TryParse(cells[5].Trim('"'), out var durationResult) ? durationResult : 0;
                var desc = cells[6].Trim('"');

                var spellBase = AssetDatabase.LoadAssetAtPath<SpellBase>(PathsAndURL.CreateSpellBasePath(spellKey));
                var spellIcon = AssetDatabase.LoadAssetAtPath<Sprite>(PathsAndURL.CreateSpellIconPath(spellKey));

                var data = new SpellData(
                    spellKey,
                    spellName,
                    cost,
                    attribute,
                    spellType,
                    duration,
                    desc,
                    spellBase,
                    spellIcon
                );
                _spellDictionary.TryAdd(spellKey, data);
            }
        }
#endif
    }
}