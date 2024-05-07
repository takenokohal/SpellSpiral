using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace HomeScene
{
    public class HomeStateChangeController : IInitializable
    {
        [Inject] private readonly MissionController _missionController;
        [Inject] private readonly HomeMenuController _homeMenuController;

        private float _missionDefaultPosition;


        public void Initialize()
        {
            _missionDefaultPosition = _missionController.transform.position.x;
            
            _homeMenuController.OnMoveToMission.Subscribe(_ =>
            {
                _homeMenuController.Close();
                _missionController.Open();

                _missionController.transform.DOMoveX(_homeMenuController.transform.position.x, 0.2f);
            });

            _missionController.OnExit.Subscribe(_ =>
            {
                _missionController.Close();
                _homeMenuController.Open();

                _missionController.transform.DOMoveX(_missionDefaultPosition, 0.2f);
            });
            
            UniTask.Void(async () =>
            {
                await UniTask.WaitUntil(() => _missionController.IsInitialized);
                _missionController.Close();

                await UniTask.WaitUntil(() => _homeMenuController.IsiInitialized);
                _homeMenuController.Open();
            });
            
        }
    }
}