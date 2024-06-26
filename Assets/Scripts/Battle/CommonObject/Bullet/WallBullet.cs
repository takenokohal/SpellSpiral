using Battle.Attack;
using Databases;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class WallBullet : DirectionalBullet, IAttackHittable
    {
        [SerializeField] private ParticleSystem hitEffect;

        private AttackDatabase _attackDatabase;

        public void Init(AttackDatabase attackDatabase)
        {
            _attackDatabase = attackDatabase;
        }

        public void OnAttacked(AttackHitController attackHitController)
        {
            hitEffect.Play();
        }

        public bool CheckHit(AttackHitController attackHitController)
        {
            return _attackDatabase.Find(attackHitController.AttackKey).OwnerType == OwnerType.Enemy;
        }
    }
}