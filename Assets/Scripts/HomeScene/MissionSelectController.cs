using System.Collections.Generic;
using System.Linq;
using Battle.Character;
using Battle.System.Main;
using Cysharp.Threading.Tasks;
using Databases;
using DG.Tweening;
using Others.Dialog;
using Others.Input;
using Others.Message;
using Others.Scene;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace HomeScene
{
    public class MissionSelectController : MonoBehaviour
    {
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly MyInputManager _myInputManager;


        [SerializeField] private MissionSelectItem missionSelectItemOrigin;
        [SerializeField] private Transform itemParent;


        private readonly List<MissionSelectItem> _missionSelectItemInstances = new();


        [SerializeField] private Transform cursor;

        private int _currentValue;

        [SerializeField] private List<CanvasGroup> missionArts;

        [Inject] private readonly HomeStateController _homeStateController;
        [SerializeField] private CanvasGroup parentCanvasGroup;

        [Inject] private readonly ChoiceDialog _choiceDialog;
        [Inject] private readonly MessageManager _messageManager;

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
                instance.Text = _messageManager.GetCharacterName(characterData.CharacterKey);
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
            if (_choiceDialog.IsOpen)
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
            var key = "mission_start_yes_no";
            var result = await _choiceDialog.StartYesNoDialog(key);
            if (result == YesNo.No)
                return;

            var missionKey = _missionSelectItemInstances[_currentValue].CharacterData.CharacterKey;
            BattleInitializer.StaticStageKey = missionKey;
            await _mySceneManager.ChangeSceneAsync("BattleMain");
        }

        private void MoveFunc()
        {
            if (!_myInputManager.IsTriggerY())
                return;
            var dir = _myInputManager.GetDirectionInputInt().y;

            var nextValue = _currentValue - dir;
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
        }
    }
}