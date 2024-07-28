using System.Collections.Generic;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using Others.Message;
using TMPro;
using UnityEngine;
using VContainer;

namespace Others.Dialog
{
    public class ChoiceDialog : MonoBehaviour
    {
        [SerializeField] private TMP_Text descriptionText;
        [SerializeField] private Transform parent;
        [SerializeField] private TMP_Text choiceTextOrigin;

        [SerializeField] private Transform cursor;

        [Inject] private readonly MessageManager _messageManager;
        [Inject] private readonly MyInputManager _myInputManager;


        [SerializeField] private CanvasGroup canvasGroup;

        private readonly List<TMP_Text> _choiceInstances = new();

        public bool IsOpen { get; private set; }

        private UniTaskCompletionSource<int> _completionSource;
        private ChoiceDialogData _currentDialogData;

        private int _currentIndex;

        private bool _fading;


        private void Start()
        {
            gameObject.SetActive(false);
            foreach (Transform child in parent)
            {
                child.gameObject.SetActive(false);
            }
        }

        //キャンセルは-1
        public async UniTask<int> StartDialog(ChoiceDialogData choiceDialogData)
        {
            IsOpen = true;
            _currentIndex = 0;

            _currentDialogData = choiceDialogData;
            _completionSource = new UniTaskCompletionSource<int>();
            CreateInstances(_currentDialogData);

            await canvasGroup.DOFade(0, 0);
            gameObject.SetActive(true);

            _fading = true;
            await canvasGroup.DOFade(1, 0.2f);
            _fading = false;


            var result = await _completionSource.Task;

            _completionSource = null;
            _currentDialogData = null;


            _fading = true;
            await canvasGroup.DOFade(0, 0.2f);
            _fading = false;

            IsOpen = false;
            gameObject.SetActive(false);
            return result;
        }

        public async UniTask<YesNo> StartYesNoDialog(string descriptionTextKey)
        {
            var v = await StartDialog(ChoiceDialogData.CreateYesNoDialog(descriptionTextKey));

            //キャンセルはNO
            if (v < 0)
                return YesNo.No;
            return (YesNo)v;
        }

        public UniTask StartYesDialog(string descriptionTextKey)
        {
            return StartDialog(ChoiceDialogData.CreateYesDialog(descriptionTextKey));
        }

        private void CreateInstances(ChoiceDialogData choiceDialogData)
        {
            foreach (var tmpText in _choiceInstances)
            {
                Destroy(tmpText.gameObject);
            }

            _choiceInstances.Clear();

            descriptionText.text = _messageManager.GetMessage(choiceDialogData.DescriptionTextKey);

            foreach (var choiceTextKey in choiceDialogData.ChoiceTextKeys)
            {
                var instance = Instantiate(choiceTextOrigin, parent);
                instance.text = _messageManager.GetMessage(choiceTextKey);
                instance.gameObject.SetActive(true);
                _choiceInstances.Add(instance);
            }
        }

        private void Update()
        {
            if (!IsOpen)
                return;

            if (!_fading)
            {
                TryMove();
                TrySelect();
            }


            UpdateView();
        }

        private void UpdateView()
        {
            var y = _choiceInstances[_currentIndex].transform.position.y;
            var pos = cursor.position;

            cursor.position = new Vector3(pos.x, y, pos.z);
        }

        private void TryMove()
        {
            if (!_myInputManager.IsTriggerY())
                return;

            var dir = -_myInputManager.GetDirectionInputInt().y;
            var nextValue = _currentIndex + dir;

            if (nextValue < 0 || nextValue >= _currentDialogData.ChoiceTextKeys.Count)
                return;

            _currentIndex = nextValue;
        }


        private void TrySelect()
        {
            var yes = _myInputManager.PlayerInput.actions["Yes"].WasPressedThisFrame();
            var no = _myInputManager.PlayerInput.actions["No"].WasPressedThisFrame();

            if (no)
            {
                _completionSource.TrySetResult(-1);
                return;
            }

            if (yes)
            {
                _completionSource.TrySetResult(_currentIndex);
            }
        }
    }
}