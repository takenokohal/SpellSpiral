using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Battle.UI
{
    public class PlayerLifeView : SerializedMonoBehaviour
    {
        [SerializeField] private Slider lifeGage;
        [SerializeField] private Slider redGage;

        [Inject] private readonly PlayerCore _playerCore;


        [SerializeField] private float redGageStopTime;
        [SerializeField] private float redGageMoveDuration;

        private Sequence _sequence;

        private void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            const int maxLife = PlayerParameter.MaxLife;
            lifeGage.maxValue = maxLife;
            lifeGage.value = maxLife;

            redGage.maxValue = maxLife;
            redGage.value = maxLife;

            await _playerCore.WaitUntilInitialize();

            var param = _playerCore.PlayerParameter;
            param.LifeObservable.Subscribe(OnLifeChange).AddTo(this);
        }

        private void OnLifeChange(float value)
        {
            lifeGage.value = value;

            _sequence?.Kill();

            _sequence = DOTween.Sequence();
            _sequence.AppendInterval(redGageStopTime);
            _sequence.Append(redGage.DOValue(value, redGageMoveDuration));
        }
    }
}