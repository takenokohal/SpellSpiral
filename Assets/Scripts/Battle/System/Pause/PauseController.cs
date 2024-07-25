using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using Others.Scene;
using Others.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace Battle.System.Pause
{
    public class PauseController : MonoBehaviour
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;

        private Choice _currentChoice;
        private bool _isActive;

        [SerializeField] private IconAndTitle[] iconAndTitles;
        [SerializeField] private Transform pointer;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

        [SerializeField] private CanvasGroup canvasGroup;


        [Inject] private readonly MySceneManager _mySceneManager;

        private float _preInput;
        private const float Threshold = 0.5f;

        [Serializable]
        private class IconAndTitle
        {
            public Image icon;
            public TMP_Text title;
        }


        private void Start()
        {
            UpdateView();
        }

        private void Update()
        {
            if (PlayerInput.actions["Pause"].WasPressedThisFrame())
                Activate(!_isActive).Forget();

            if (!_isActive)
                return;
            if (_mySceneManager.Changing)
                return;


            TryCancelPause();
            MovePointer();
            TryPressButton().Forget();

            _preInput = PlayerInput.actions["Vertical"].ReadValue<float>();
        }

        public async UniTaskVoid Activate(bool isActive)
        {
            _isActive = isActive;
            Time.timeScale = isActive ? 0 : 1;

            await UniTask.Yield();
           // _myInputManager.BattleInput.enabled = isActive;
           _myInputManager.PlayerInput.SwitchCurrentActionMap(isActive ? "UI" : "Player");

            await canvasGroup.DOFade(isActive ? 1 : 0, 0.1f).SetUpdate(true);
        }

        private void MovePointer()
        {
            var inputAction = PlayerInput.actions["Vertical"];
            var value = inputAction.ReadValue<float>();

            if (!IsTrigger())
                return;

            var dir = value switch
            {
                <= -0.5f => 1,
                >= 0.5f => -1,
                _ => 0
            };

            _currentChoice = _currentChoice.Increment(dir);

            UpdateView();
        }

        private bool IsTrigger()
        {
            var value = PlayerInput.actions["Vertical"].ReadValue<float>();
            switch (_preInput)
            {
                case < Threshold when value >= Threshold:
                case > -Threshold when value <= -Threshold:
                    return true;
                default:
                    return false;
            }
        }

        private void TryCancelPause()
        {
            var noButton = PlayerInput.actions["No"];
            if (!noButton.triggered)
                return;

            Activate(false).Forget();
        }

        private async UniTaskVoid TryPressButton()
        {
            var yesButton = PlayerInput.actions["Yes"];

            if (!yesButton.triggered)
                return;

            switch (_currentChoice)
            {
                case Choice.Restart:
                    Activate(false).Forget();
                    break;
                case Choice.Home:
                    Time.timeScale = 1;
                    await _mySceneManager.ChangeSceneAsync("Home");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //_myInputManager.BattleInput.enabled = true;
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