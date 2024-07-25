using Battle.Attack;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
using Battle.System.Main;
using Databases;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Battle.Character.Servant
{
    public class CharacterFactory 
    {
        [Inject] private readonly AttackHitEffectFactory _attackHitEffectFactory;
        [Inject] private readonly AttackDatabase _attackDatabase;
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly AllCharacterManager _allCharacterManager;
        [Inject] private readonly BattleLoop _battleLoop;
        [Inject] private readonly MagicCircleFactory _magicCircleFactory;
        [Inject] private readonly CharacterCamera _characterCamera;
        [Inject] private readonly ReadyEffectFactory _readyEffectFactory;
        [Inject] private readonly CameraSwitcher _cameraSwitcher;
        [Inject] private readonly ConstValues _constValues;

        public CharacterBase CreateAndInject(string characterKey, CharacterBase master, Vector2 pos)
        {
            var characterPrefab = _characterDatabase.Find(characterKey).CharacterBase;
            var instance = Object.Instantiate(characterPrefab, pos, Quaternion.identity);
         
            instance.AcquiredInject(
                master,
                _attackHitEffectFactory,
                _attackDatabase,
                _characterDatabase,
                _allCharacterManager,
                _battleLoop,
                _magicCircleFactory,
                this,
                _readyEffectFactory,
                _characterCamera,
                _cameraSwitcher,
                 _constValues);
            instance.InitCharacterData(characterKey);


            instance.transform.position = pos;

            return instance;
        }
    }
}