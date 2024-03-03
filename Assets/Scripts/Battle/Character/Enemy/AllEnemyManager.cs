using System.Collections.Generic;
using Battle.Attack;
using UniRx;
using UnityEngine;

namespace Battle.Character.Enemy
{
    public class AllEnemyManager
    {
     //   [Inject] private readonly CinemachineTargetGroup _targetGroup;

        private readonly ReactiveCollection<EnemyCore> _enemyCores = new();
        public IReadOnlyReactiveCollection<EnemyCore> EnemyCores => _enemyCores;

        public void RegisterEnemy(EnemyCore enemyCore) => _enemyCores.Add(enemyCore);
        public void RemoveEnemy(EnemyCore enemyCore) => _enemyCores.Remove(enemyCore);

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
        }
    }
}