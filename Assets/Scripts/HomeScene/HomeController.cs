using System;
using Audio;
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

namespace HomeScene
{
    public class HomeController : MonoBehaviour
    {
        [Inject] private MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.UiInput;

        private Choice _currentChoice;

        [SerializeField] private IconAndTitle[] iconAndTitles;
        [SerializeField] private Transform pointer;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

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
            if (_mySceneManager.Changing)
                return;

            MovePointer();
            TryChangeScene().Forget();

            _preInput = PlayerInput.actions["Vertical"].ReadValue<float>();
        }

        private void MovePointer()
        {
            var ver = PlayerInput.actions["Vertical"];
            var value = ver.ReadValue<float>();
            if (!IsTrigger())
                return;

            var dir = value switch
            {
                <= -0.5f => 1,
                >= 0.5f => -1,
                _ => 0
            };

            _currentChoice = _currentChoice.Increment(dir);

            var selected = iconAndTitles[(int)_currentChoice];
            foreach (var iconAndTitle in iconAndTitles)
            {
                var isSelected = iconAndTitle == selected;
                var color = isSelected ? selectedColor : unselectedColor;
                iconAndTitle.icon.DOColor(color, 0.2f);
                iconAndTitle.title.DOColor(color, 0.2f);
            }

            pointer.transform.DOMove(selected.title.transform.position, 0);

            AllAudioManager.PlaySe("CursorMove");
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

        private async UniTask TryChangeScene()
        {
            var inputAction = PlayerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            var nextScene = _currentChoice switch
            {
                Choice.Mission => "BalteciaStage",
                Choice.Training => "Training",
                Choice.EditDeck => "EditDeck",
                _ => throw new ArgumentOutOfRangeException()
            };

            AllAudioManager.PlaySe("Select");
            await _mySceneManager.ChangeSceneAsync(nextScene);
        }

        private void UpdateView()
        {
            var selected = iconAndTitles[(int)_currentChoice];
            foreach (var iconAndTitle in iconAndTitles)
            {
                var isSelected = iconAndTitle == selected;
                var color = isSelected ? selectedColor : unselectedColor;
                iconAndTitle.icon.DOColor(color, 0.2f);
                iconAndTitle.title.DOColor(color, 0.2f);
            }

            pointer.transform.DOMove(selected.title.transform.position, 0.2f);
        }

        private enum Choice
        {
            Mission,
            Training,
            EditDeck
        }
    }
}