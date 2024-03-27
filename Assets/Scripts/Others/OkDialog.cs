using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Others
{
    public class OkDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        [SerializeField] private PlayerInput myPlayerInput;

        [SerializeField] private Transform root;
        
        private UniTaskCompletionSource _completionSource;

        public bool IsOpen { get; private set; }

        private void Start()
        {
            root.localScale=Vector3.zero;
            gameObject.SetActive(false);
        }

        public async UniTask Open(string message)
        {
            var otherInput
                = FindObjectsOfType<PlayerInput>().Where(value => value != myPlayerInput);
            var playerInputs = otherInput as PlayerInput[] ?? otherInput.ToArray();
            foreach (var playerInput in playerInputs)
            {
                playerInput.enabled = false;
            }
            gameObject.SetActive(true);
            IsOpen = true;
            messageText.text = message;
            await root.DOScale(1, 0.2f);
            _completionSource = new UniTaskCompletionSource();

            await _completionSource.Task;


            _completionSource = null;
            IsOpen = false;

            await root.DOScale(0, 0.2f);
            
            foreach (var playerInput in playerInputs)
            {
                playerInput.enabled = true;
            }

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_completionSource == null)
                return;

            if (myPlayerInput.actions["Yes"].WasPressedThisFrame())
                _completionSource.TrySetResult();
            else if (myPlayerInput.actions["No"].WasPressedThisFrame())
                _completionSource.TrySetResult();
        }

    }
}