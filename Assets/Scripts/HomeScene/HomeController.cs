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

namespace HomeScene
{
    public class HomeController : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private Choice _currentChoice;

        [SerializeField] private IconAndTitle[] iconAndTitles;
        [SerializeField] private Transform pointer;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

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
            if (_mySceneManager.Changing)
                return;

            MovePointer();
            TryChangeScene().Forget();
        }

        private void MovePointer()
        {
            var ver = _playerInput.actions["Vertical"];
            if (!ver.triggered)
                return;

            var value = ver.ReadValue<float>();
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
        }

        private async UniTask TryChangeScene()
        {
            var inputAction = _playerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            var nextScene = _currentChoice switch
            {
                Choice.Mission => "Stage1",
                Choice.Training => "Training",
                Choice.EditDeck => "EditDeck",
                _ => throw new ArgumentOutOfRangeException()
            };

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