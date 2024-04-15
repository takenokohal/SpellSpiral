using System.Collections.Generic;
using System.Linq;
using Battle.Attack;
using UniRx;

namespace Battle.Character.Enemy
{
    public class AllCharacterManager
    {
        private readonly ReactiveCollection<CharacterBase> _allCharacters = new();
        public IReadOnlyReactiveCollection<CharacterBase> AllCharacters => _allCharacters;


        public void RegisterCharacter(CharacterBase characterBase) => _allCharacters.Add(characterBase);
        public void RemoveCharacter(CharacterBase characterBase) => _allCharacters.Remove(characterBase);


        private readonly ReactiveProperty<EnemyBase> _boss = new();
        public EnemyBase Boss => _boss.Value;
        public void RegisterBoss(EnemyBase enemyBase) => _boss.Value = enemyBase;

        public IEnumerable<CharacterBase> GetEnemyCharacters() =>
            AllCharacters.Where(value => value.CharacterData.OwnerType == OwnerType.Enemy);

        public IEnumerable<CharacterBase> GetPlayerCharacters() =>
            AllCharacters.Where(value => value.CharacterData.OwnerType == OwnerType.Player);


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