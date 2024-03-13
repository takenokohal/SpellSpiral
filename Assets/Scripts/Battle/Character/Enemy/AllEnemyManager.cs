using System.Collections.Generic;
using Battle.Attack;
using UniRx;
using UnityEngine;

namespace Battle.Character.Enemy
{
    public class AllEnemyManager
    {
     //   [Inject] private readonly CinemachineTargetGroup _targetGroup;

        private readonly ReactiveCollection<EnemyBase> _enemyCores = new();
        public IReadOnlyReactiveCollection<EnemyBase> EnemyCores => _enemyCores;

        public void RegisterEnemy(EnemyBase enemyBase) => _enemyCores.Add(enemyBase);
        public void RemoveEnemy(EnemyBase enemyBase) => _enemyCores.Remove(enemyBase);

        private readonly ReactiveProperty<EnemyBase> _boss = new();
        public EnemyBase Boss => _boss.Value;
        public void RegisterBoss(EnemyBase enemyBase) => _boss.Value = enemyBase;
        


        /*
        private readonly ReactiveCollection<AttackHitController> _attacks = new();

        public IReadOnlyCollection<AttackHitController> Attacks => _attacks;

        public void RegisterAttack(AttackHitController attackHitController)
        {
            Debug.Log(attackHitController);
            _attacks.Add(attackHitController);
         //   _targetGroup.AddMember(attackHitController.transform, 1, 1);
        }

        public void RemoveAttack(AttackHitController attackHitController)
        {
            _attacks.Remove(attackHitController);
        //    _targetGroup.RemoveMember(attackHitController.transform);
        }*/
    }
}