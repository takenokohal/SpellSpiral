using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

namespace Battle.Character.Player.Buff
{
    public class PlayerBuff
    {
        public ReactiveCollection<BuffParameter> BuffParameters { get; } = new();

        public int BuffCount(BuffKey buffKey) => BuffParameters.Count(value => value.BuffKey == buffKey);
        

        public void MyFixedUpdate()
        {
            var dt = Time.fixedDeltaTime;
            var list = new List<BuffParameter>();
            foreach (var buffParameter in BuffParameters)
            {
                buffParameter.CountUp(dt);
                if(buffParameter.IsFinished)
                    list.Add(buffParameter);
            }
            
            foreach (var buffParameter in list)
            {
                BuffParameters.Remove(buffParameter);
            }
        }
    }
}