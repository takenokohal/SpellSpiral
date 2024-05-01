using System.Linq;
using Battle.Character.Player;
using Battle.Character.Servant;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle.Servant
{
    public class AssassinServant : ServantBase
    {
        [SerializeField] private float speed;
        private static readonly int Animation1 = Animator.StringToHash("animation");

        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            
            Animator.SetInteger(Animation1, 2);
            Animator.speed = 2;
            LookAtTarget(AllCharacterManager.PlayerCore);
            Rigidbody.velocity = new Vector3(CharacterRotation.Rotation * speed, 0);
        }

        private void FixedUpdate()
        {
            var positionX = Rigidbody.position.x;
            var b = CharacterRotation.IsRight && positionX > 10f;
            var b2 = !CharacterRotation.IsRight && positionX < -10f;

            if (!b && !b2)
                return;

            CharacterRotation.IsRight = !CharacterRotation.IsRight;
            
            var v = Rigidbody.velocity;
            v.x *= -1;
            Rigidbody.velocity = v;
        }
    }
}