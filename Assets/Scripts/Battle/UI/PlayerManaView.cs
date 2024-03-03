using Battle.Character.Player;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Battle.UI
{
    public class PlayerManaView : SerializedMonoBehaviour
    {
        [Inject] private readonly PlayerCore _playerCore;

        [SerializeField] private Image manaCircle;


        private void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            await _playerCore.WaitUntilInitialize();
            var param = _playerCore.PlayerParameter;

            param.ManaObservable.Subscribe(OnChange).AddTo(this);
        }

        private void OnChange(float value)
        {
            manaCircle.fillAmount = value / PlayerParameter.MaxMana;
        }
    }
}