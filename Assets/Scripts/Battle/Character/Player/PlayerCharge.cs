using Audio;
using Battle.Character.Player.Buff;
using UnityEngine;

namespace Battle.Character.Player
{
    public class PlayerCharge : PlayerComponent
    {
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private ParticleSystem subEffect;


        [SerializeField] private Color normalEffectColor;
        [SerializeField] private Color buffedEffectColor;

        private static readonly int AnimKey = Animator.StringToHash("Charging");

        private bool Chargeable => !IsPlayerDead &&
                                   !PlayerParameter.Warping;

        private SeSource _seSource;

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


            if (!Charging)
                TryStartCharge();

            if (Charging)
                TryEndCharge();
        }

        private void FixedUpdate()
        {
            if (IsPlayerDead)
                return;

            if (Charging)
                QuickCharge();

            if (!Charging)
                AutoCharge();
            
            TryAutoHeal();
        }


        private void TryStartCharge()
        {
            if (!Chargeable)
                return;

            if (!PlayerCore.PlayerInput.actions["Charge"].IsPressed())
                return;

            effect.Play();
            _seSource = AllAudioManager.PlaySe("Charge");
            Charging = true;
            PlayerCore.Animator.SetBool(AnimKey, true);
        }

        private void TryEndCharge()
        {
            if (PlayerCore.PlayerInput.actions["Charge"].IsPressed())
                return;

            effect.Stop();
            _seSource.Stop();
            Charging = false;
            PlayerCore.Animator.SetBool(AnimKey, false);
        }

        private void AutoCharge()
        {
            var chargeValue = PlayerConstData.AutoManaChargePerSec;

            var buffCount =PlayerBuff.BuffCount(BuffKey.AutoManaCharge);

            for (int i = 0; i < buffCount; i++)
            {
                chargeValue *= PlayerConstData.BuffAutoManaChargeRatio;
            }

            PlayerParameter.Mana += Time.fixedDeltaTime * chargeValue;
        }

        private void QuickCharge()
        {
            var chargeValue = PlayerConstData.QuickManaChargePerSec;
            var buffCount = PlayerBuff.BuffCount(BuffKey.QuickManaCharge);

            for (int i = 0; i < buffCount; i++)
            {
                chargeValue *= PlayerConstData.BuffQuickManaChargeRatio;
            }

            PlayerParameter.Mana += Time.fixedDeltaTime * chargeValue;
        }

        private void TryAutoHeal()
        {
            var healCount = PlayerBuff.BuffCount(BuffKey.AutoHeal);

            if (healCount <= 0)
                return;

            var value = 0f;

            for (int i = 0; i < healCount; i++)
            {
                value += PlayerConstData.AttachedAutoHealValue;
            }

            PlayerCore.CurrentLife += value;
        }
    }
}