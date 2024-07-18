using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Attack;
using Battle.Character;
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
    [CreateAssetMenu(menuName = "Create CharacterDatabase", fileName = "CharacterDatabase", order = 0)]
    public class CharacterDatabase : SerializedScriptableObject
    {
        [OdinSerialize, Searchable] private readonly Dictionary<string, CharacterData> _characterDictionary = new();

        public IReadOnlyDictionary<string, CharacterData> CharacterDictionary => _characterDictionary;


        public CharacterData Find(string characterKey) => CharacterDictionary[characterKey];


#if UNITY_EDITOR

        public static CharacterDatabase LoadOnEditor()
        {
            var path = PathsAndURL.CreateDatabasePath<CharacterDatabase>();
            var v = AssetDatabase.LoadAssetAtPath<CharacterDatabase>(path);
            return v;
        }

        [Button]
        public void Update()
        {
            UpdateAsync().Forget();
        }

        private async UniTaskVoid UpdateAsync()
        {
            Debug.Log("UpdateStart");
            var req = UnityWebRequest.Get(PathsAndURL.CharacterDatabaseSpreadSheetURL);
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
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        private void Parse(string csv)
        {
            _characterDictionary.Clear();

            var rows = csv.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var row in rows.Skip(1))
            {
                var cells = row.Split(new[] { ',' });

                var characterKey = cells[0].Trim('"');

                var nameJp = cells[1].Trim('"');
                var nameEn = cells[2].Trim('"');

                var names = new Dictionary<SystemLanguage, string>()
                {
                    { SystemLanguage.Japanese, nameJp },
                    { SystemLanguage.English, nameEn }
                };

                var characterType = Enum.TryParse<CharacterType>(cells[3].Trim('"'), out var ctResult)
                    ? ctResult
                    : CharacterType.Player;

                var masterName = cells[4].Trim('"');

                var ownerType = Enum.TryParse<OwnerType>(cells[5].Trim('"'), out var otResult)
                    ? otResult
                    : OwnerType.Player;

                var life = int.TryParse(cells[6].Trim('"'), out var lifeResult) ? lifeResult : 0;


                var characterBase =
                    AssetDatabase.LoadAssetAtPath<CharacterBase>(PathsAndURL.CreateCharacterBasePath(characterKey));
                var stageObject =
                    AssetDatabase.LoadAssetAtPath<GameObject>(PathsAndURL.CreateStageObjectPath(characterKey));

                var data = new CharacterData(characterKey, names, characterType, masterName, ownerType, life,
                    characterBase, null, stageObject);

                _characterDictionary.TryAdd(characterKey, data);
            }
        }
#endif
    }
}