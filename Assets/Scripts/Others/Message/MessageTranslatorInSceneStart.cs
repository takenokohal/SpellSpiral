using System.Linq;
using Cysharp.Threading.Tasks;
using DeckEdit.SaveData;
using TMPro;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Others.Message
{
    public class MessageTranslatorInSceneStart : IInitializable
    {
        [Inject] private readonly MessageManager _messageManager;

        public void Initialize()
        {
            TranslateAllDefaultText().Forget();
        }

        private async UniTaskVoid TranslateAllDefaultText()
        {
            await UniTask.WaitUntil(() => _messageManager.IsInitialized);
            
            var texts = Object.FindObjectsByType<TMP_Text>(FindObjectsSortMode.None);
            foreach (var tmpText in texts.Where(value => value.text.StartsWith("$$")))
            {
                var key = tmpText.text[2..];
                tmpText.text = _messageManager.GetMessage(key);
            }
        }
    }
}