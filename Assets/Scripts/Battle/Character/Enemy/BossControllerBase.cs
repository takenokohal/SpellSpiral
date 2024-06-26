using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Battle.Attack;
using Battle.Character.Player.Buff;
using Battle.MyCamera;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Others;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using VContainer;

namespace Battle.Character.Enemy
{
    public abstract class BossControllerBase<T> : BossBase where T : Enum
    {
        [SerializeField] private List<BossSequenceBase<T>> sequencePrefabs;
        private readonly Dictionary<T, BossSequenceBase<T>> _sequenceInstances = new();

        protected T CurrentState { get; private set; }

        


        protected override void InitializeFunction()
        {
            base.InitializeFunction();
            
            CreateSequence();
        }

        protected UniTask PlayState(T nextState)
        {
            CurrentState = nextState;
            return _sequenceInstances[CurrentState].SequenceStart();
        }



        private void CreateSequence()
        {
            foreach (var prefab in sequencePrefabs)
            {
                _sequenceInstances.Add(prefab.StateKey, prefab.Construct(
                    new BossSequenceBase<T>.SequenceRequiredComponents()
                    {
                        AllCharacterManager = AllCharacterManager,
                        Parent = this,
                        PlayerCore = PlayerCore,
                        CameraSwitcher = CameraSwitcher,
                        MagicCircleFactory = MagicCircleFactory,
                        CharacterFactory = CharacterFactory,
                        ReadyEffectFactory = ReadyEffectFactory
                    }));
            }
        }

    }
}