using System.Linq;
using Battle.Character.Player;
using Battle.Character.Servant;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle.Servant
{
    public class HomingServant : ServantBase
    {
        [SerializeField] private float maxSpeed;

        [SerializeField] private float changeSpeedValue;

        private CharacterBase _target;

        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            _target = AllCharacterManager.PlayerCore;
        }


        private void FixedUpdate()
        {
            var to = GetDirectionToTarget(_target) * maxSpeed;

            LookAtTarget(_target);

            Rigidbody.velocity = Vector3.MoveTowards(Rigidbody.velocity, to, changeSpeedValue);
        }
    }
}