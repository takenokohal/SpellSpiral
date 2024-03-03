using System;
using Others;
using UnityEngine;

namespace Battle.Attack
{
    [Serializable]
    public class AttackData
    {
        [SerializeField] private int damage;
        [SerializeField] private AttackWeight attackWeight;
        [SerializeField] private SpellAttribute attribute;

        public AttackData(int damage, AttackWeight attackWeight, SpellAttribute attribute)
        {
            this.damage = damage;
            this.attackWeight = attackWeight;
            this.attribute = attribute;
        }


        public int Damage => damage;

        public AttackWeight AttackWeight => attackWeight;

        public SpellAttribute Attribute => attribute;
    }
}