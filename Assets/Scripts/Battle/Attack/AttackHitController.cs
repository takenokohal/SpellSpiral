using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Battle.Attack
{
    public class AttackHitController : SerializedMonoBehaviour
    {
        [SerializeField] private OwnerType owner;

        [SerializeField] private AttackKey attackKey;

        public AttackKey AttackKey => attackKey;

        private readonly Subject<IAttackHittable> _onAttackHit = new();
        public IObservable<IAttackHittable> OnAttackHit => _onAttackHit.TakeUntilDestroy(this);

        private void OnTriggerEnter(Collider other)
        {
            var v = other.GetComponent<IAttackHittable>();

            if (v == null)
                return;

            if (v.GetOwnerType() == owner)
                return;

            v.OnAttacked(this);

            _onAttackHit.OnNext(v);
        }
    }
}