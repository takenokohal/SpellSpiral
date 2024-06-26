using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using HomeScene;
using Others;
using Others.Input;
using Others.Scene;
using Others.Utils;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace Battle.CommonObject.Result
{
    public class LoseMenu : MonoBehaviour
    {
        private bool _isActive;
        
        [Inject] private readonly MySceneManager _mySceneManager;

        [Inject] private readonly MyInputManager _myInputManager;

        [SerializeField] private VerticalChoiceListView verticalChoiceListView;
        [SerializeField] private IconAndTitle.Parameter[] parameters;


        public async UniTaskVoid Activate()
        {
            verticalChoiceListView.Initialize(_myInputManager, parameters);
            gameObject.SetActive(true);
            await transform.DOScaleY(0, 0);
            await transform.DOScaleY(1, 0.5f);
            _isActive = true;
            verticalChoiceListView.OnSelect.Where(_ => _isActive).Subscribe(OnSelect).AddTo(this);
        }


        /*
        private void Update()
        {
            if (!_isActive)
                return;
            if (_mySceneManager.Changing)
                return;

            MovePointer();
            TryChangeScene();
        }

        private void MovePointer()
        {
            var inputAction = PlayerInput.actions["Move"];
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
            var inputAction = PlayerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            var nextScene = _currentChoice switch
            {
                Choice.Retry => _mySceneManager.CurrentSceneName,
                Choice.EditDeck => "EditDeck",
                Choice.Home => "Home",
                _ => throw new ArgumentOutOfRangeException()
            };

            _mySceneManager.ChangeSceneAsync(nextScene).Forget();
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
        */

        private void OnSelect(int i)
        {
            var key = parameters[i].title;

            if (key == "Retry")
            {
                key = _mySceneManager.CurrentSceneName;
            }

            verticalChoiceListView.enabled = false;
            _mySceneManager.ChangeSceneAsync(key).Forget();
        }
    }
}