using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Others.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace Others
{
    [CreateAssetMenu(menuName = "Create MessageDatabase", fileName = "MessageDatabase", order = 0)]
    public class MessageDatabase : SerializedScriptableObject
    {
        [Serializable]
        public class MessageData
        {
            [SerializeField] private string messageKey;
            [SerializeField] private string jpText;

            public string MessageKey => messageKey;
            public string JpText => jpText;

            public MessageData(string messageKey, string jpText)
            {
                this.messageKey = messageKey;
                this.jpText = jpText;
            }
        }

        [OdinSerialize] private Dictionary<string, MessageData> _messages;
        public IReadOnlyDictionary<string, MessageData> Messages => _messages;

        public MessageData Find(string key) => Messages[key];


#if UNITY_EDITOR
        public static MessageDatabase LoadOnEditor()
        {
            var path = PathsAndURL.CreateDatabasePath<MessageDatabase>();
            var v = AssetDatabase.LoadAssetAtPath<MessageDatabase>(path);
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
            var req = UnityWebRequest.Get(PathsAndURL.MessageDatabaseSpreadSheetURL);
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
            _messages.Clear();

            var rows = csv.Split(new[] { "\n" }, StringSplitOptions.None);

            foreach (var row in rows.Skip(1))
            {
                var cells = row.Split(new[] { ',' });

                var messageKey = cells[0].Trim('"');
                var jpText = cells[1].Trim('"');

                _messages.TryAdd(messageKey, new MessageData(messageKey, jpText));
            }
        }
#endif
    }
}