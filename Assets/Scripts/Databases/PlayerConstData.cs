using UnityEngine;

namespace Databases
{
    [CreateAssetMenu(menuName = "Create PlayerConstData", fileName = "PlayerConstData", order = 0)]
    public class PlayerConstData : ScriptableObject
    {
        [SerializeField] private float autoManaChargePerSec;
        [SerializeField] private float quickManaChargePerSec;

        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveLerpValue;
        [SerializeField] private float chargingMoveSpeed;

        [SerializeField] private float chantDuration;

        [SerializeField] private float stepSpeed;
        [SerializeField] private float stepDuration;
        [SerializeField] private float stepDrag;

        [SerializeField] private float buffReduceManaRatio;
        [SerializeField] private float buffAutoManaChargeRatio;
        [SerializeField] private float buffQuickManaChargeRatio;
        [SerializeField] private float attachedAutoHealValue;
        [SerializeField] private float buffPowerRatio;
        [SerializeField] private float buffDefenseRatio;
        

        public float AutoManaChargePerSec => autoManaChargePerSec;
        public float QuickManaChargePerSec => quickManaChargePerSec;
        public float MoveSpeed => moveSpeed;

        public float ChantDuration => chantDuration;

        public float MoveLerpValue => moveLerpValue;
        public float ChargingMoveSpeed => chargingMoveSpeed;
        public float StepSpeed => stepSpeed;
        public float StepDuration => stepDuration;

        public float StepDrag => stepDrag;

        public float BuffPowerRatio => buffPowerRatio;

        public float BuffDefenseRatio => buffDefenseRatio;

        public float BuffQuickManaChargeRatio => buffQuickManaChargeRatio;

        public float BuffReduceManaRatio => buffReduceManaRatio;

        public float BuffAutoManaChargeRatio => buffAutoManaChargeRatio;

        public float AttachedAutoHealValue => attachedAutoHealValue;
    }
}