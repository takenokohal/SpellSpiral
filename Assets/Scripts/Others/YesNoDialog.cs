using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Others
{
    public class YesNoDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        [SerializeField] private PlayerInput myPlayerInput;

        private readonly UniTaskCompletionSource<YesNo> _completionSource = new();

        public async UniTask<YesNo> Open(string message)
        {
            messageText.text = message;
            await transform.DOScale(1, 0.2f);

            var v = await _completionSource.Task;

            return v;
        }

        private void Update()
        {
            if (myPlayerInput.actions["Yes"].WasPressedThisFrame())
                _completionSource.TrySetResult(YesNo.Yes);
            else if (myPlayerInput.actions["No"].WasPressedThisFrame())
                _completionSource.TrySetResult(YesNo.No);
        }


        public enum YesNo
        {
            Yes,
            No
        }
    }
}