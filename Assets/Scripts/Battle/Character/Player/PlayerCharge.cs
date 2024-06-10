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
        
        private bool CantCharge => IsPlayerDead ||
                                   PlayerParameter.Warping||
                                   PlayerParameter.SpellChanting;

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
            if (CantCharge)
                return;

            if (!PlayerCore.PlayerInput.actions["Charge"].IsPressed())
                return;

            effect.Play();
            _seSource = AllAudioManager.PlaySe("Charge");
            Charging = true;
            
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Charge);
        }

        private void TryEndCharge()
        {
            if (PlayerCore.PlayerInput.actions["Charge"].IsPressed())
                return;

            effect.Stop();
            effect.Clear();
            _seSource.Stop();
            Charging = false;
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Idle);
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