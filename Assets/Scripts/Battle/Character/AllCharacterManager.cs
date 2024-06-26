using System.Collections.Generic;
using System.Linq;
using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Cinemachine;
using UniRx;
using UnityEngine;

namespace Battle.Character
{
    public class AllCharacterManager
    {
        private readonly CinemachineTargetGroup _targetGroup;
        private readonly ReactiveCollection<CharacterBase> _allCharacters = new();
        public IReadOnlyReactiveCollection<CharacterBase> AllCharacters => _allCharacters;


        public bool IsPlayerAndBossRegistered()
        {
            return PlayerCore != null && Boss != null;
        }

        public void RegisterCharacter(CharacterBase characterBase)
        {
            _allCharacters.Add(characterBase);

            if (characterBase is PlayerCore playerCore)
            {
                PlayerCore = playerCore;
            }

            if (_targetGroup.m_Targets.Any(value => value.target == characterBase.transform))
                return;
            _targetGroup.AddMember(characterBase.transform, 1, 0);
        }

        public void RemoveCharacter(CharacterBase characterBase)
        {
            _allCharacters.Remove(characterBase);
            _targetGroup.RemoveMember(characterBase.transform);
        }


        private readonly ReactiveProperty<BossBase> _boss = new();
        public BossBase Boss => _boss.Value;
        public void RegisterBoss(BossBase bossBase) => _boss.Value = bossBase;

        public IEnumerable<CharacterBase> GetEnemyCharacters() =>
            AllCharacters.Where(value => value.CharacterData.OwnerType == OwnerType.Enemy);

        public IEnumerable<CharacterBase> GetPlayerCharacters() =>
            AllCharacters.Where(value => value.CharacterData.OwnerType == OwnerType.Player);

        public PlayerCore PlayerCore { get; private set; }

        public AllCharacterManager()
        {
            _targetGroup = Object.FindObjectOfType<CinemachineTargetGroup>();
        }
    }
}