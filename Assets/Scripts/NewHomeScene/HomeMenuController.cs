using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DeckEdit.Controller;
using DG.Tweening;
using Others;
using Others.Input;
using Others.Utils;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace NewHomeScene
{
    public class HomeMenuController : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<HomeMenuType, HomeMenuItem> _items = new();

        private int _currentValue;
        private HomeMenuType CurrentType => (HomeMenuType)_currentValue;

        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;

        private float _preInput;
        private const float Threshold = 0.5f;

        [Inject] private readonly MessageDatabase _messageDatabase;

        [SerializeField] private TMP_Text descriptionText;

        [Inject] private readonly HomeStateController _homeStateController;

        [SerializeField] private CanvasGroup parentCanvasGroup;

        private void Start()
        {
            _homeStateController.StateObservable.Subscribe(OnStateChange).AddTo(this);
            SetValue(0);
        }

        private void OnStateChange(HomeStateController.HomeState homeState)
        {
            var toFade = homeState == HomeStateController.HomeState.MainMenu ? 1 : 0;
            parentCanvasGroup.DOFade(toFade, 0.2f);
        }

        private void Update()
        {
            if (_homeStateController.CurrentState != HomeStateController.HomeState.MainMenu)
                return;

            if (DeckEditActiveController.IsActive)
                return;

            MoveFunc();
            ClickFunc();
        }

        private void ClickFunc()
        {
            var v = PlayerInput.actions["Yes"].WasPressedThisFrame();
            if (!v)
                return;

            switch (CurrentType)
            {
                case HomeMenuType.Mission:
                    _homeStateController.CurrentState = HomeStateController.HomeState.MissionSelect;
                    break;
                case HomeMenuType.DeckEdit:
                    DeckEditActiveController.StartDeckEdit().Forget();
                    break;
                case HomeMenuType.Training:
                    break;
                case HomeMenuType.Tutorial:
                    break;
                case HomeMenuType.System:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void MoveFunc()
        {
            var value = PlayerInput.actions["vertical"].ReadValue<float>();


            switch (_preInput)
            {
                case < Threshold when value >= Threshold:
                case > -Threshold when value <= -Threshold:
                    _preInput = value;
                    break;
                default:
                    _preInput = value;
                    return;
            }

            var dir = value switch
            {
                <= -0.5f => 1,
                >= 0.5f => -1,
                _ => 0
            };
            TryMove(dir);
        }

        private void TryMove(int moveDirection)
        {
            var nextValue = _currentValue + moveDirection;
            var length = EnumUtil<HomeMenuType>.GetValues().Count();

            if (nextValue < 0 || nextValue >= length)
                return;

            SetValue(nextValue);
        }

        private void SetValue(int nextValue)
        {
            _items[CurrentType].transform.DOScale(1, 0.2f);
            _currentValue = nextValue;
            _items[CurrentType].transform.DOScale(1.5f, 0.2f);


            var messageKey = CurrentType switch
            {
                HomeMenuType.Mission => "home_mission_description",
                HomeMenuType.DeckEdit => "home_deck_edit_description",
                HomeMenuType.Training => "home_training_description",
                HomeMenuType.Tutorial => "home_tutorial_description",
                HomeMenuType.System => "home_system_description",
                _ => throw new ArgumentOutOfRangeException()
            };
            descriptionText.text = _messageDatabase.Find(messageKey).JpText;
        }
    }
}