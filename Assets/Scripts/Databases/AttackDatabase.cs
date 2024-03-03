using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Attack;
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
    [CreateAssetMenu(menuName = "Create AttackDatabase", fileName = "AttackDatabase", order = 0)]
    public class AttackDatabase : SerializedScriptableObject
    {
        [OdinSerialize]
        private readonly Dictionary<string, Dictionary<string, AttackData>> _attackDataDictionary = new();

        public IReadOnlyDictionary<string, Dictionary<string, AttackData>> AttackDataDictionary =>
            _attackDataDictionary;

        public AttackData Find(AttackKey attackKey)
        {
            return _attackDataDictionary[attackKey.CharacterKey][attackKey.DetailKey];
        }


#if UNITY_EDITOR
        public static AttackDatabase LoadOnEditor()
        {
            var path = PathsAndURL.CreateDatabasePath<AttackDatabase>();
            var v = AssetDatabase.LoadAssetAtPath<AttackDatabase>(path);
            return v;
        }

        [Button]
        private void Update()
        {
            UpdateAsync().Forget();
        }

        private async UniTaskVoid UpdateAsync()
        {
            Debug.Log("UpdateStart");
            var req = UnityWebRequest.Get(PathsAndURL.AttackDatabaseSpreadSheetURL);
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
            _attackDataDictionary.Clear();

            var rows = csv.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var row in rows.Skip(1))
            {
                var cells = row.Split(new[] { ',' });

                var characterKey = cells[0].Trim('"');
                var attackKey = cells[1].Trim('"');
                var damage = int.TryParse(cells[2].Trim('"'), out var damageResult) ? damageResult : 0;
                var attribute = Enum.TryParse<SpellAttribute>(cells[3].Trim('"'), out var attributeResult)
                    ? attributeResult
                    : SpellAttribute.Fire;
                var weight = Enum.TryParse<AttackWeight>(cells[4].Trim('"'), out var weightResult)
                    ? weightResult
                    : AttackWeight.Light;

                _attackDataDictionary.TryAdd(characterKey, new Dictionary<string, AttackData>());

                _attackDataDictionary[characterKey].TryAdd(attackKey, new AttackData(damage, weight, attribute));
            }
        }
#endif
    }
}