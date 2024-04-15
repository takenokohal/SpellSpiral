using Audio;
using Battle.Attack;
using Battle.Character.Player;
using Battle.MyCamera;
using Cinemachine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace Battle.Character.Enemy
{
    public abstract class EnemyBase : CharacterBase
    {
        [Inject] protected readonly PlayerCore playerCore;
        [Inject] protected readonly SpecialCameraSwitcher specialCameraSwitcher;


        public Vector2 GetDirectionToPlayer()
        {
            return playerCore.GetDirectionToPlayer(transform.position);
        }
        


        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            this.OnDestroyAsObservable().Subscribe(_ => AllCharacterManager.RemoveCharacter(this)).AddTo(this);
        }
        

        public void LookPlayer()
        {
            CharacterRotation.Rotation = GetDirectionToPlayer().x;
        }
    }
}