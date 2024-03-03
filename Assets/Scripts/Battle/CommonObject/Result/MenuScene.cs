using System;
using DG.Tweening;
using Others;
using Others.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Battle.CommonObject.Result
{
    public class MenuScene : MonoBehaviour
    {
        private PlayerInput _playerInput;

        private Choice _currentChoice;

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


        private void Start()
        {
            _playerInput = GetComponent<PlayerInput>();
            UpdateView();
        }

        private void Update()
        {
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

        private void TryChangeScene()
        {
            var inputAction = _playerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            var nextScene = _currentChoice switch
            {
                Choice.Mission => "Mission",
                Choice.Training => "Training",
                Choice.EditDeck => "EditDeck",
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
            Mission,
            Training,
            EditDeck
        }
    }
}