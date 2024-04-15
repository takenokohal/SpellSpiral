﻿using System.Collections.Generic;
using Battle.Attack;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.CommonObject.MagicCircle;
using Battle.MyCamera;
using Databases;
using Others;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using VContainer;

namespace Battle.Character.Servant
{
    public class ServantFactory : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<string, ServantBase> _servantPrefabs;

        [Inject] private readonly AttackHitEffectFactory _attackHitEffectFactory;
        [Inject] private readonly AttackDatabase _attackDatabase;
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly AllCharacterManager _allCharacterManager;
        [Inject] private readonly GameLoop _gameLoop;
        [Inject] private readonly MagicCircleFactory _magicCircleFactory;
        [Inject] private readonly CharacterCamera _characterCamera;

        public ServantBase Create(string key)
        {
            var prefab = _servantPrefabs[key];
            var instance = prefab.CreateFromPrefab();
            instance.AcquiredInject(
                _attackHitEffectFactory,
                _attackDatabase,
                _characterDatabase,
                _allCharacterManager,
                _gameLoop,
                _magicCircleFactory,
                this,
                _characterCamera);

            return instance;
        }
    }
}