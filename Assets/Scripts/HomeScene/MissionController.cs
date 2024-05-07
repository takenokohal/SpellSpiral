using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Input;
using Others.Scene;
using UniRx;
using UnityEngine;
using VContainer;

namespace HomeScene
{
    public class MissionController : MonoBehaviour
    {
        private VerticalChoiceListView _verticalChoiceListView;

        private CanvasGroup _canvasGroup;

        [Inject] private readonly MySceneManager _mySceneManager;
        [Inject] private readonly MyInputManager _myInputManager;
        [SerializeField] private List<IconAndTitle.Parameter> parameters;


        private bool _isOpen;

        private readonly Subject<Unit> _onExit = new();
        public IObservable<Unit> OnExit => _onExit.TakeUntilDestroy(this);
        
        public bool IsInitialized { get; private set; }

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _verticalChoiceListView = GetComponent<VerticalChoiceListView>();

            _verticalChoiceListView.Initialize(_myInputManager, parameters);
            _verticalChoiceListView.OnSelect.Where(_ => _isOpen).Subscribe(OnSelect).AddTo(this);

            _verticalChoiceListView.OnExit.Where(_ => _isOpen).Subscribe(_ => _onExit.OnNext(Unit.Default));
            IsInitialized = true;
        }

        public void Open()
        {
            _canvasGroup.DOFade(1, 0.2f);
            _verticalChoiceListView.enabled = true;

            _isOpen = true;
        }

        public void Close()
        {
            _canvasGroup.DOFade(0, 0.2f);
            _verticalChoiceListView.enabled = false;

            _isOpen = false;
        }


        private void OnSelect(int i)
        {
            var nextScene = parameters[i].title;

            _verticalChoiceListView.enabled = false;
            Close();
            _mySceneManager.ChangeSceneAsync(nextScene).Forget();
        }
    }
}