using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Scene;
using Others.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace Battle.CommonObject.Pause
{
    public class PauseController : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private Choice _currentChoice;
        private bool _isActive;

        [SerializeField] private IconAndTitle[] iconAndTitles;
        [SerializeField] private Transform pointer;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

        [SerializeField] private CanvasGroup canvasGroup;


        [Inject] private readonly MySceneManager _mySceneManager;

        [Serializable]
        private class IconAndTitle
        {
            public Image icon;
            public TMP_Text title;
        }


        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            UpdateView();
        }

        private void Update()
        {
            if (_playerInput.actions["Pause"].WasPressedThisFrame())
                Activate(!_isActive).Forget();

            if (!_isActive)
                return;
            if (_mySceneManager.Changing)
                return;

            MovePointer();
            TryPressButton();
        }

        public async UniTaskVoid Activate(bool isActive)
        {
            _isActive = isActive;
            Time.timeScale = isActive ? 0 : 1;


            await canvasGroup.DOFade(isActive ? 1 : 0, 0.1f).SetUpdate(true);
        }

        private void MovePointer()
        {
            var inputAction = _playerInput.actions["Vertical"];
            if (!inputAction.triggered)
                return;

            var value = inputAction.ReadValue<float>();
            var dir = value switch
            {
                <= -0.5f => 1,
                >= 0.5f => -1,
                _ => 0
            };

            _currentChoice = _currentChoice.Increment(dir);

            UpdateView();
        }

        private void TryPressButton()
        {
            var inputAction = _playerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            switch (_currentChoice)
            {
                case Choice.Restart:
                    Activate(false).Forget();
                    break;
                case Choice.Home:
                    Time.timeScale = 1;
                    _mySceneManager.ChangeSceneAsync("Home").Forget();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UpdateView()
        {
            var selected = iconAndTitles[(int)_currentChoice];
            foreach (var iconAndTitle in iconAndTitles)
            {
                var isSelected = iconAndTitle == selected;
                var color = isSelected ? selectedColor : unselectedColor;
                iconAndTitle.icon.DOColor(color, 0.2f).SetUpdate(true);
                iconAndTitle.title.DOColor(color, 0.2f).SetUpdate(true);
            }

            pointer.transform.DOMove(selected.title.transform.position, 0.2f).SetUpdate(true);
        }

        private enum Choice
        {
            Restart,
            Home
        }
    }
}