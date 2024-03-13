using System.Linq;
using Battle.Character.Enemy;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Battle.UI
{
    public class BossLifeView : SerializedMonoBehaviour
    {
        [SerializeField] private Slider lifeGage;
        [SerializeField] private Slider redGage;

        [Inject] private readonly AllEnemyManager _allEnemyManager;


        [SerializeField] private float redGageStopTime;
        [SerializeField] private float redGageMoveDuration;

        private Sequence _sequence;

        private async void Start()
        {
            await UniTask.WaitWhile(() => _allEnemyManager.Boss== null);
            Init(_allEnemyManager.Boss);
        }

        private void Init(EnemyBase enemyBase)
        {
            var param = enemyBase.EnemyParameter;
            var maxLife = param.MaxLife;
            lifeGage.maxValue = maxLife;
            lifeGage.value = maxLife;

            redGage.maxValue = maxLife;
            redGage.value = maxLife;

            param.CurrentLifeObservable.Subscribe(OnLifeChange).AddTo(this);
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