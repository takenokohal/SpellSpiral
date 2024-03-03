using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using Others.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Battle.CommonObject.Result
{
    public class LoseMenu : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private Choice _currentChoice;
        private bool _isActive;

        [SerializeField] private IconAndTitle[] iconAndTitles;
        [SerializeField] private Transform pointer;

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;


        [Serializable]
        private class IconAndTitle
        {
            public Image icon;
            public TMP_Text title;
        }


        public async UniTaskVoid Activate()
        {
            await transform.DOScaleY(0, 0);
            await transform.DOScaleY(1, 0.5f);
            _isActive = true;
        }

        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            UpdateView();
        }

        private void Update()
        {
            if (!_isActive)
                return;
            if (SceneChanger.Fading)
                return;

            MovePointer();
            TryChangeScene();
        }

        private void MovePointer()
        {
            var inputAction = _playerInput.actions["Move"];
            if (!inputAction.triggered)
                return;

            var value = inputAction.ReadValue<Vector2>().y;
            var dir = value switch
            {
                <= -0.5f => 1,
                >= 0.5f => -1,
                _ => 0
            };

            _currentChoice = _currentChoice.Increment(dir);

            UpdateView();
        }

        private void TryChangeScene()
        {
            var inputAction = _playerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            var nextScene = _currentChoice switch
            {
                Choice.Retry => SceneChanger.CurrentSceneName,
                Choice.EditDeck => "EditDeck",
                Choice.Home => "Home",
                _ => throw new ArgumentOutOfRangeException()
            };

            SceneChanger.ChangeSceneAsync(nextScene);
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
            Retry,
            EditDeck,
            Home
        }
    }
}