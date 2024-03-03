using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battle.Character.Player.Buff
{
    public class PlayerBuff : PlayerComponent
    {
        private readonly ReactiveCollection<BuffParameter> _buffParameters = new();
        public IReadOnlyReactiveCollection<BuffParameter> BuffParameters => _buffParameters;

        public bool HasBuff(BuffKey buffKey) => _buffParameters.Any(value => value.BuffKey == buffKey);

        public void SetBuff(BuffParameter buffParameter)
        {
            var first = BuffParameters.FirstOrDefault(value => value.BuffKey == buffParameter.BuffKey);
            if (first == null)
            {
                _buffParameters.Add(buffParameter);
            }
            else
            {
                first.ResetTime(buffParameter.CurrentTime);
            }
        }

        protected override void Init()
        {
        }

        private void Update()
        {
            if (!IsBattleStarted)
                return;

            var dt = Time.deltaTime;

            var tmpList = new List<BuffParameter>(_buffParameters);

            foreach (var buffParameter in tmpList)
            {
                buffParameter.CountUp(dt);
                if (buffParameter.IsFinished)
                    _buffParameters.Remove(buffParameter);
            }
        }
    }
}