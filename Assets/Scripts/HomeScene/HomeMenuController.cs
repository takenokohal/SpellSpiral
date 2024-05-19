using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using Others.Input;
using Others.Scene;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace HomeScene
{
    public class HomeMenuController : MonoBehaviour
    {
        private VerticalChoiceListView _verticalChoiceListView;

        [Inject] private readonly MySceneManager _mySceneManager;
        [Inject] private readonly MyInputManager _myInputManager;

        [SerializeField] private List<IconAndTitle.Parameter> parameters;
        [Inject] private readonly YesNoDialog _yesNoDialog;

        private CanvasGroup _canvasGroup;

        private readonly Subject<Unit> _onMoveToMission = new();
        public IObservable<Unit> OnMoveToMission => _onMoveToMission.TakeUntilDestroy(this);

        private bool _isOpened;

        public bool IsiInitialized { get; private set; }

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _verticalChoiceListView = GetComponent<VerticalChoiceListView>();

            _verticalChoiceListView.Initialize(_myInputManager, parameters);
            _verticalChoiceListView.OnSelect.Where(_ => _isOpened).Subscribe(OnSelect)
                .AddTo(this);
            _verticalChoiceListView.OnExit.Where(_ => _isOpened)
                .Subscribe(_ => { TryExit().Forget(); }).AddTo(this);

            IsiInitialized = true;
        }


        public void Open()
        {
            _canvasGroup.DOFade(1, 0.2f);
            _verticalChoiceListView.enabled = true;
            _isOpened = true;
        }

        public void Close()
        {
            _canvasGroup.DOFade(0, 0.2f);
            _verticalChoiceListView.enabled = false;

            _isOpened = false;
        }

        private async UniTaskVoid TryExit()
        {
            _verticalChoiceListView.enabled = false;
            var result = await _yesNoDialog.Open("ゲームを終了しますか？");
            _verticalChoiceListView.enabled = true;
            if (result == YesNoDialog.YesNo.Yes)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
             Application.Quit();

#endif
            }
        }

        private void OnSelect(int i)
        {
            var currentChoice = (Choice)i;
            if (currentChoice == Choice.Mission)
            {
                _onMoveToMission.OnNext(Unit.Default);
                return;
            }

            var nextScene = currentChoice switch
            {
                Choice.Training => "Training",
                Choice.EditDeck => "EditDeck",
                _ => throw new ArgumentOutOfRangeException()
            };

            _verticalChoiceListView.enabled = false;
            _mySceneManager.ChangeSceneAsync(nextScene).Forget();
        }


        private enum Choice
        {
            Mission,
            Training,
            EditDeck
        }
    }
}