using UnityEngine;

namespace Battle.Character.Player.Buff
{
    public class BuffParameter
    {
        public BuffKey BuffKey { get; }
        public float EffectDuration { get; private set; }

        public float CurrentTime { get; private set; }

        public BuffParameter(BuffKey buffKey, float effectDuration)
        {
            BuffKey = buffKey;
            EffectDuration = effectDuration;
        }

        public void CountUp(float deltaTime)
        {
            CurrentTime += deltaTime;
        }

        public void ResetTime(float duration) => EffectDuration = duration;

        public bool IsFinished => CurrentTime > EffectDuration;
        
    }
}