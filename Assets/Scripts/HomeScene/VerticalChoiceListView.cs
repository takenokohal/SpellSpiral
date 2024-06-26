using System;
using System.Collections.Generic;
using Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HomeScene
{
    public class VerticalChoiceListView : MonoBehaviour
    {
        private MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;


        private int _currentIndex;

        [SerializeField] private IconAndTitle iconAndTitlePrefab;


        [SerializeField] private Transform parent;

        [SerializeField] private Transform pointer;


        private readonly List<IconAndTitle> _iconAndTitles = new();

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;


        private float _preInput;
        private const float Threshold = 0.5f;

        private readonly Subject<int> _onSelect = new();
        public IObservable<int> OnSelect => _onSelect.TakeUntilDestroy(this);

        private readonly Subject<Unit> _onExit = new();
        public IObservable<Unit> OnExit => _onExit.TakeUntilDestroy(this);

        private bool _isInitialized;

        public void Initialize(MyInputManager myInputManager, IEnumerable<IconAndTitle.Parameter> parameters)
        {
            //デフォルトの子要素を全て非表示
            foreach (Transform t in parent)
            {
                t.gameObject.SetActive(false);
            }

            _myInputManager = myInputManager;
            CreateInstances(parameters);

            _isInitialized = true;
        }


        private void Update()
        {
            if (!_isInitialized)
                return;

            TryMovePointer();
            TrySelect();
            TryExit();

            _preInput = PlayerInput.actions["vertical"].ReadValue<float>();
        }


        private void CreateInstances(IEnumerable<IconAndTitle.Parameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                var instance = iconAndTitlePrefab.CreateFromPrefab(parameter, parent);
                _iconAndTitles.Add(instance);
            }

            UniTask.Void(async () =>
            {
                await UniTask.Delay(100);
                UpdateView();
            });
        }


        private void TryMovePointer()
        {
            if (!IsTrigger())
                return;

            var value = PlayerInput.actions["vertical"].ReadValue<float>();

            var dir = value switch
            {
                <= -0.5f => 1,
                >= 0.5f => -1,
                _ => 0
            };


            _currentIndex = Mathf.Clamp(_currentIndex + dir, 0, _iconAndTitles.Count - 1);

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

        private void TrySelect()
        {
            var inputAction = PlayerInput.actions["Yes"];
            if (!inputAction.triggered)
                return;

            AllAudioManager.PlaySe("Select");

            _onSelect.OnNext(_currentIndex);
        }

        private void TryExit()
        {
            var inputAction = PlayerInput.actions["No"];
            if (!inputAction.triggered)
                return;

            AllAudioManager.PlaySe("Cancel");

            _onExit.OnNext(Unit.Default);
        }


        private void UpdateView()
        {
            var selected = _iconAndTitles[_currentIndex];
            foreach (var iconAndTitle in _iconAndTitles)
            {
                var isSelected = iconAndTitle == selected;
                var color = isSelected ? selectedColor : unselectedColor;
                iconAndTitle.Image.DOColor(color, 0.2f);
                iconAndTitle.TitleText.DOColor(color, 0.2f);
            }

            pointer.transform.DOMoveY(selected.TitleText.transform.position.y, 0.2f);

            AllAudioManager.PlaySe("CursorMove");
        }
    }
}