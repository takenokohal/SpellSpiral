using System;
using System.Collections.Generic;
using System.Linq;
using Battle.Character.Player.Buff;
using UniRx;
using UnityEngine;

namespace Battle.Character.Player
{
    public class PlayerParameter
    {
        public const int MaxLife = 100;

        private readonly ReactiveProperty<float> _life = new(MaxLife);

        public float Life
        {
            get => _life.Value;
            set => _life.Value = Mathf.Clamp(value, 0, MaxLife);
        }

        public IObservable<float> LifeObservable => _life;

        public bool IsDead => Life <= 0;

        public const int MaxMana = 100;

        private readonly ReactiveProperty<float> _mana = new(MaxMana);

        public float Mana
        {
            get => _mana.Value;
            set => _mana.Value = Mathf.Clamp(value, 0, MaxMana);
        }

        public IObservable<float> ManaObservable => _mana;


        public bool Invincible { get; set; }

        public bool SpellChanting { get; set; }
        public bool QuickCharging { get; set; }
        public bool Warping { get; set; }


    }
}