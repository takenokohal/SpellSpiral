using System.Threading;
using Audio;
using Battle.Attack;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.MyCamera;
using Battle.System.Main;
using Cinemachine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace Battle.Character.Enemy
{
    public abstract class BossBase : CharacterBase
    {
        protected PlayerCore PlayerCore => AllCharacterManager.PlayerCore;


        public Vector2 ToAnimationVelocity { get; set; }

        public WizardAnimationController WizardAnimationController { get; private set; }

        protected readonly CancellationTokenSource commonCancellationTokenSource = new();


        public Vector2 GetDirectionToPlayer()
        {
            return PlayerCore.GetDirectionToPlayer(Rigidbody.position);
        }


        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            WizardAnimationController = new WizardAnimationController(Animator);

            this.OnDestroyAsObservable().Subscribe(_ => AllCharacterManager.RemoveCharacter(this)).AddTo(this);


            AllCharacterManager.RegisterBoss(this);

            Observable.EveryFixedUpdate().Subscribe(_ => MyFixedUpdate()).AddTo(this);


            BattleLoop.Event
                .Where(value => value == BattleEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => { BattleStart().Forget(); })
                .AddTo(this);
        }


        private async UniTaskVoid BattleStart()
        {
            await WaitUntilInitialize();

            gameObject.OnDestroyAsObservable().Subscribe(_ => commonCancellationTokenSource.Cancel());

            OnDeadObservable().Take(1).Subscribe(_ =>
            {
                commonCancellationTokenSource.Cancel();
                BattleLoop.SendEvent(BattleEvent.Win);
            });
        }

        private void MyFixedUpdate()
        {
            if (BattleLoop.CurrentState != BattleEvent.BattleStart)
                return;

            if (AnimatorIsNull)
                return;

            WizardAnimationController.HorizontalSpeedValue = Mathf.Lerp(WizardAnimationController.HorizontalSpeedValue,
                ToAnimationVelocity.x * CharacterRotation.Rotation, 0.2f);
        }


        public void LookPlayer()
        {
            CharacterRotation.Rotation = GetDirectionToPlayer().x;
        }

        protected override float CalcDamage(AttackHitController attackHitController)
        {
            var attackBuff = PlayerCore.PlayerBuff.BuffCount(BuffKey.BuffMultiply);
            var damage = AttackDatabase.Find(attackHitController.AttackKey).Damage;
            for (int i = 0; i < attackBuff; i++)
            {
                damage *= PlayerCore.PlayerConstData.BuffPowerRatio;
            }

            return damage;
        }
    }
}