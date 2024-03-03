using Battle.Character.Player.Buff;
using UnityEngine;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerCharge : PlayerComponent
    {
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private ParticleSystem subEffect;


        [SerializeField] private Color normalEffectColor;
        [SerializeField] private Color buffedEffectColor;


        [Inject] private readonly PlayerBuff _playerBuff;

        private static readonly int AnimKey = Animator.StringToHash("Charging");

        private bool Chargeable => !PlayerParameter.IsDead &&
                                   !PlayerParameter.Warping;

        private bool Charging
        {
            get => PlayerParameter.QuickCharging;
            set => PlayerParameter.QuickCharging = value;
        }


        protected override void Init()
        {
        }

        private void Update()
        {
            if (!IsBattleStarted)
                return;
            Charge();

            if (!Charging)
                TryStartCharge();

            if (Charging)
                TryEndCharge();
        }


        private void TryStartCharge()
        {
            if (!Chargeable)
                return;

            if (!PlayerCore.PlayerInput.actions["Charge"].IsPressed())
                return;


            var effectMain = effect.main;
            effectMain.startColor = _playerBuff.HasBuff(BuffKey.QuickManaCharge)
                ? buffedEffectColor
                : normalEffectColor;
            var subEffectMain = subEffect.main;
            subEffectMain.startColor = _playerBuff.HasBuff(BuffKey.QuickManaCharge)
                ? buffedEffectColor
                : normalEffectColor;
            effect.Play();
            Charging = true;
            PlayerCore.Animator.SetBool(AnimKey, true);
        }

        private void TryEndCharge()
        {
            if (PlayerCore.PlayerInput.actions["Charge"].IsPressed())
                return;

            effect.Stop();
            Charging = false;
            PlayerCore.Animator.SetBool(AnimKey, false);
        }

        private void Charge()
        {
            if (IsPlayerDead)
                return;

            var finalValue = 0f;
            var autoCharge = PlayerConstData.AutoManaChargePerSec;
            if (_playerBuff.HasBuff(BuffKey.AutoManaCharge))
                autoCharge *= PlayerConstData.BuffAutoManaChargeRatio;

            finalValue += autoCharge;

            if (Charging)
            {
                var quickCharge = PlayerConstData.QuickManaChargePerSec;
                if (_playerBuff.HasBuff(BuffKey.QuickManaCharge))
                    quickCharge *= PlayerConstData.BuffQuickManaChargeRatio;

                finalValue += quickCharge;
            }

            PlayerCore.PlayerParameter.Mana += Time.deltaTime * finalValue;
        }
    }
}