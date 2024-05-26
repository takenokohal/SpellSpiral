﻿using System.Collections.Generic;
using System.Linq;
using Battle.Character.Enemy.Variables.Dorothy;
using Cysharp.Threading.Tasks;
using Others;
using Others.Utils;
using UniRx;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class EschalotController : BossBase<EschalotState>
    {
        private bool _halfLifeSpecialAttacked;

        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            GameLoop.Event
                .Where(value => value ==GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ => Loop().Forget())
                .AddTo(this);
        }

        private async UniTask Loop()
        {
         //   await PlayState(DorothyState.FlowerGarden);
         
            while (!commonCancellationTokenSource.IsCancellationRequested)
            {
                var nextState = EschalotState.ResignBeam;

                LookPlayer();

                await PlayState(nextState);
            }
        }

        private bool HalfLifeCheck()
        {
            return CurrentLife <= CharacterData.Life / 2f && !_halfLifeSpecialAttacked;
        }
        
    }
}