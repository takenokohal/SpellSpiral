using System;
using System.Collections.Generic;
using System.Linq;
using Battle;
using Battle.Character;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Others;
using Others.Dialog;
using Others.Input;
using Others.Scene;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace NewHomeScene
{
    public class MissionSelectController : MonoBehaviour
    {
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;
        private float _preInput;
        private const float Threshold = 0.5f;


        [SerializeField] private MissionSelectItem missionSelectItemOrigin;
        [SerializeField] private Transform itemParent;


        private readonly List<MissionSelectItem> _missionSelectItemInstances = new();


        [SerializeField] private Transform cursor;

        private int _currentValue;

        [SerializeField] private List<CanvasGroup> missionArts;

        [Inject] private readonly HomeStateController _homeStateController;
        [SerializeField] private CanvasGroup parentCanvasGroup;

        [Inject] private readonly YesNoDialog _yesNoDialog;
        [Inject] private readonly MessageDatabase _messageDatabase;

        [Inject] private readonly MySceneManager _mySceneManager;

        private void Start()
        {
            foreach (Transform child in itemParent)
            {
                child.gameObject.SetActive(false);
            }

            GenerateList().Forget();

            _homeStateController.StateObservable.Subscribe(OnStateMove).AddTo(this);
        }

        private void OnStateMove(HomeStateController.HomeState homeState)
        {
            var yes = homeState == HomeStateController.HomeState.MissionSelect;
            parentCanvasGroup.DOFade(yes ? 1 : 0, 0.2f);
            parentCanvasGroup.transform.DOLocalMoveX(yes ? 0 : 100, 0.2f);
        }

        private async UniTaskVoid GenerateList()
        {
            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

            var characterDataList = _characterDatabase.CharacterDictionary
                .Where(value => value.Value.CharacterType == CharacterType.Boss)
                .Where(value => value.Key != "TestMan" && value.Key != "TrainingCubeMan")
                .Select(value => value.Value).ToList();

            foreach (var characterData in characterDataList)
            {
                var instance = Instantiate(missionSelectItemOrigin, itemParent);
                instance.Init(characterData);
                instance.Text = characterData.CharacterNameJp;
                instance.gameObject.SetActive(true);

                _missionSelectItemInstances.Add(instance);
            }

            await UniTask.Yield(PlayerLoopTiming.FixedUpdate);

            SetValue(0);
        }

        private void Update()
        {
            if (_homeStateController.CurrentState != HomeStateController.HomeState.MissionSelect)
                return;
            if (_yesNoDialog.IsOpen)
                return;


            MoveFunc();
            ButtonFunc();
        }

        private void ButtonFunc()
        {
            var yes = _myInputManager.PlayerInput.actions["Yes"].WasPressedThisFrame();
            var no = _myInputManager.PlayerInput.actions["No"].WasPressedThisFrame();

            if (yes)
            {
                TryMissionStart().Forget();
                return;
            }

            if (no)
            {
                _homeStateController.CurrentState = HomeStateController.HomeState.MainMenu;
                return;
            }
        }

        private async UniTaskVoid TryMissionStart()
        {
            var message = _messageDatabase.Find("mission_start_yes_no");
            var result = await _yesNoDialog.Open(message.JpText);
            if (result == YesNoDialog.YesNo.No)
                return;

            var missionKey = _missionSelectItemInstances[_currentValue].CharacterData.CharacterKey;
            BattleInitializer.StaticStageName = missionKey;
            await _mySceneManager.ChangeSceneAsync("BattleMain");
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
            var length = _missionSelectItemInstances.Count;

            if (nextValue < 0 || nextValue >= length)
                return;

            SetValue(nextValue);
        }

        private void SetValue(int nextValue)
        {
            _missionSelectItemInstances[_currentValue].SetLineSize(1f);
            var oldArt = missionArts[_currentValue];
            oldArt.DOFade(0f, 0.2f);

            _currentValue = nextValue;

            var item = _missionSelectItemInstances[_currentValue];
            var toPos = item.transform.position;

            item.SetLineSize(1.2f);
            cursor.transform.DOMoveY(toPos.y, 0.2f);

            var newArt = missionArts[_currentValue];
            newArt.DOFade(1f, 0.2f);

            // descriptionText.text = _messageDatabase.Find(messageKey).JpText;
        }
    }
}