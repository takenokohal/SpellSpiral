/*using System;
using UniRx;

namespace Battle.Character.Enemy
{
    public class EnemyParameter
    {
        public float MaxLife { get; }
        
        private readonly ReactiveProperty<float> _currentLife = new();
        public IObservable<float> CurrentLifeObservable => _currentLife;

        public float CurrentLife
        {
            get => _currentLife.Value;
            set => _currentLife.Value = value;
        }

        public EnemyParameter(float maxLife)
        {
            MaxLife = maxLife;
            _currentLife.Value = MaxLife;
        }
    }
}*/