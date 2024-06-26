using Battle.Attack;
using Battle.Character.Player.Buff;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class GrazeMaster : SpellBase, IAttackHittable
    {
        [SerializeField] private ParticleSystem buffEffect;

        [SerializeField] private float manaAddValue;

        private bool _isActive;

        protected override async UniTaskVoid Init()
        {
            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(PlayerCore, 1, () => PlayerCore.transform.position));

            _isActive = true;

            var ed = SpellData.EffectDuration;
            PlayerBuff.BuffParameters.Add(
                new BuffParameter(BuffKey.GrazeMaster, SpellData.SpellKey, ed));

            await MyDelay(ed);

            _isActive = false;
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            transform.position = PlayerCore.Rigidbody.position;
        }

        public void OnAttacked(AttackHitController attackHitController)
        {
        }

        public bool CheckHit(AttackHitController attackHitController)
        {
            if (!_isActive)
                return false;

            if (AttackDatabase.Find(attackHitController.AttackKey).OwnerType != OwnerType.Enemy)
                return false;

            buffEffect.Play();
            PlayerCore.PlayerParameter.Mana += manaAddValue;

            return false;
        }
    }
}