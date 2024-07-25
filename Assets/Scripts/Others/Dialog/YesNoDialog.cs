/*using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using Others.Message;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Others.Dialog
{
    public class YesNoDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        [SerializeField] private Transform root;

        [Inject] private readonly MyInputManager _myInputManager;

        private UniTaskCompletionSource<YesNo> _completionSource;
        [SerializeField] private CanvasGroup parentCanvasGroup;

        [SerializeField] private TMP_Text yesNoText;

        [Inject] private readonly MessageManager _messageManager;
        
        

        public bool IsOpen { get; private set; }

        private void Start()
        {
            root.localScale = new Vector3(0, 1, 1);
            gameObject.SetActive(false);

        //    yesNoText.text = _messageDatabase.Find("yes_no_dialog").JpText;
        }

        public async UniTask<YesNo> Open(string message)
        {
            gameObject.SetActive(true);
            parentCanvasGroup.DOFade(1, 0.2f);
            IsOpen = true;
            messageText.text = message;
            await root.DOScaleX(1, 0.2f);
            _completionSource = new UniTaskCompletionSource<YesNo>();

            var v = await _completionSource.Task;


            _completionSource = null;

            parentCanvasGroup.DOFade(0, 0.2f);
            await root.DOScaleX(0, 0.2f);

            IsOpen = false;

            gameObject.SetActive(false);
            return v;
        }

        private void Update()
        {
            if (_completionSource == null)
                return;

            if (_myInputManager.PlayerInput.actions["Yes"].WasPressedThisFrame())
            {
                _completionSource.TrySetResult(YesNo.Yes);
                AllAudioManager.PlaySe("Select");
            }
            else if (_myInputManager.PlayerInput.actions["No"].WasPressedThisFrame())
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
}*/