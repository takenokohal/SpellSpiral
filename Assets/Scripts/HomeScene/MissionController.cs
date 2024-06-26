using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Character;
using Cysharp.Threading.Tasks;
using Databases;
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
        private readonly List<IconAndTitle.Parameter> _parameters = new();


        [Inject] private readonly CharacterDatabase _characterDatabase;
        [SerializeField] private Sprite magicCircleTest;


        private bool _isOpen;

        private readonly Subject<Unit> _onExit = new();
        public IObservable<Unit> OnExit => _onExit.TakeUntilDestroy(this);

        public bool IsInitialized { get; private set; }

        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _verticalChoiceListView = GetComponent<VerticalChoiceListView>();

            _parameters.AddRange(_characterDatabase.CharacterDictionary
                .Where(value => value.Value.CharacterType == CharacterType.Boss)
                .Where(value => value.Key != "TrainingCubeMan" && value.Key != "TestMan")
                .Select(value =>
                    new IconAndTitle.Parameter()
                    {
                        sprite = magicCircleTest,
                        title = value.Key
                    }));
            _verticalChoiceListView.Initialize(_myInputManager, _parameters);
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
            var nextScene = _parameters[i].title;

            _verticalChoiceListView.enabled = false;
            Close();
            _mySceneManager.ChangeSceneAsync(nextScene).Forget();
        }
    }
}