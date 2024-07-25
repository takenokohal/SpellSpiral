using System;
using Databases;
using Others;
using Spell;
using UnityEngine;

namespace Battle.Attack
{
    [Serializable]
    public class AttackData
    {
        [SerializeField] private OwnerType ownerType;
        [SerializeField] private float damage;
        [SerializeField] private SpellAttribute attribute;

        public AttackData(
            OwnerType ownerType,
            float damage,
            SpellAttribute attribute)
        {
            this.ownerType = ownerType;
            this.damage = damage;
            this.attribute = attribute;
        }


        public OwnerType OwnerType => ownerType;
        public float Damage => damage;

        public SpellAttribute Attribute => attribute;
    }
}