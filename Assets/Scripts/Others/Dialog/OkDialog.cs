using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using TMPro;
using UnityEngine;
using VContainer;

namespace Others.Dialog
{
    public class OkDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        [SerializeField] private Transform root;

        private UniTaskCompletionSource _completionSource;

        [Inject] private readonly MyInputManager _myInputManager;

        public bool IsOpen { get; private set; }

        private void Start()
        {
            root.localScale = Vector3.zero;
            gameObject.SetActive(false);
        }

        public async UniTask Open(string message)
        {
            gameObject.SetActive(true);
            IsOpen = true;
            messageText.text = message;
            await root.DOScale(1, 0.2f);
            _completionSource = new UniTaskCompletionSource();

            await _completionSource.Task;


            _completionSource = null;

            await root.DOScale(0, 0.2f);

            IsOpen = false;
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_completionSource == null)
                return;

            if (_myInputManager.PlayerInput.actions["Yes"].WasPressedThisFrame())
            {
                _completionSource.TrySetResult();
                AllAudioManager.PlaySe("Select");
            }

            else if (_myInputManager.PlayerInput.actions["No"].WasPressedThisFrame())
            {
                _completionSource.TrySetResult();
                AllAudioManager.PlaySe("Select");

            }
        }
    }
}