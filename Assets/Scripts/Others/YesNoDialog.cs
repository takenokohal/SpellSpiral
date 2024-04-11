using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using TMPro;
using UnityEngine;
using VContainer;

namespace Others
{
    public class YesNoDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        [SerializeField] private Transform root;

        [Inject] private readonly MyInputManager _myInputManager;

        private UniTaskCompletionSource<YesNo> _completionSource;

        public bool IsOpen { get; private set; }

        private void Start()
        {
            root.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }

        public async UniTask<YesNo> Open(string message)
        {
            gameObject.SetActive(true);
            IsOpen = true;
            messageText.text = message;
            await root.DOScale(1, 0.2f);
            _completionSource = new UniTaskCompletionSource<YesNo>();

            var v = await _completionSource.Task;


            _completionSource = null;

            await root.DOScale(0, 0.2f);

            IsOpen = false;

            gameObject.SetActive(false);
            return v;
        }

        private void Update()
        {
            if (_completionSource == null)
                return;

            if (_myInputManager.UiInput.actions["Yes"].WasPressedThisFrame())
            {
                _completionSource.TrySetResult(YesNo.Yes);
                AllAudioManager.PlaySe("Select");
            }
            else if (_myInputManager.UiInput.actions["No"].WasPressedThisFrame())
            {
                _completionSource.TrySetResult(YesNo.No);
                AllAudioManager.PlaySe("Cancel");
            }
        }


        public enum YesNo
        {
            Yes,
            No
        }
    }
}