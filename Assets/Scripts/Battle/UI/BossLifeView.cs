using System.Linq;
using Battle.Character;
using Battle.Character.Enemy;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others.Message;
using Sirenix.OdinInspector;
using TMPro;
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

        [SerializeField] private TMP_Text nameText;

        [Inject] private readonly AllCharacterManager _allCharacterManager;

        [Inject] private readonly MessageManager _messageManager;

        [SerializeField] private float redGageStopTime;
        [SerializeField] private float redGageMoveDuration;

        private Sequence _sequence;

        private async void Start()
        {
            await UniTask.WaitWhile(() => _allCharacterManager.Boss == null);
            Init(_allCharacterManager.Boss);
        }

        private void Init(BossBase bossBase)
        {
            var maxLife = bossBase.CharacterData.Life;
            lifeGage.maxValue = maxLife;
            lifeGage.value = maxLife;

            redGage.maxValue = maxLife;
            redGage.value = maxLife;

            bossBase.CurrentLifeObservable.Subscribe(OnLifeChange).AddTo(this);

            nameText.text = _messageManager.GetCharacterName(bossBase.CharacterKey);
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