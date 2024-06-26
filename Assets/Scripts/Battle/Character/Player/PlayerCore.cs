using Battle.Attack;
using Battle.Character.Player.Buff;
using Cinemachine;
using Databases;
using Others;
using Others.Input;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerCore : CharacterBase
    {
        [Inject] private readonly MyInputManager _myInputManager;
        public PlayerInput PlayerInput => _myInputManager.PlayerInput;

        [Inject] private readonly PlayerConstData _playerConstData;

        public PlayerConstData PlayerConstData => _playerConstData;

        public WizardAnimationController WizardAnimationController { get; private set; }


        public bool IsBattleStarted { get; private set; }


        public PlayerParameter PlayerParameter { get; } = new();

        public PlayerBuff PlayerBuff { get; } = new();

        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            WizardAnimationController = new WizardAnimationController(Animator);

            OnDeadObservable().Subscribe(_ =>
            {
                BattleLoop.SendEvent(BattleEvent.Lose);
                OnDead();
            }).AddTo(this);

            BattleLoop.Event.Where(value => value == BattleEvent.Win)
                .Subscribe(_ => PlayerParameter.InvincibleFlag++).AddTo(this);


            BattleLoop.Event
                .Where(value => value == BattleEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => IsBattleStarted = true);
        }

        private void FixedUpdate()
        {
            PlayerBuff.MyFixedUpdate();
        }


        private void OnDead()
        {
            var targetGroup = FindObjectOfType<CinemachineTargetGroup>();
            var deadObj = new GameObject("DeadObject")
            {
                transform =
                {
                    position = transform.position
                }
            };

            targetGroup.AddMember(deadObj.transform, 1, 0);


            gameObject.SetActive(false);
        }

        public Vector2 GetDirectionToPlayer(Vector2 from)
        {
            return ((Vector2)transform.position - from).normalized;
        }

        protected override bool ChickHitChild(AttackHitController attackHitController)
        {
            return !PlayerParameter.Invincible;
        }

        protected override float CalcDamage(AttackHitController attackHitController)
        {
            var defense = PlayerBuff.BuffCount(BuffKey.DefenseBuff);
            var damage = AttackDatabase.Find(attackHitController.AttackKey).Damage;
            for (int i = 0; i < defense; i++)
            {
                damage *= PlayerConstData.BuffDefenseRatio;
            }

            return damage;
        }
    }
}