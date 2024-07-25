using Battle.Attack;
using Battle.Character.Player.Buff;
using Battle.CommonObject.Bullet;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Spell.Variables.HighlanderSpell
{
    public class ReflectionMirror : SpellBase, IAttackHittable
    {
        [SerializeField] private SingleHitBeam beamPrefab;

        [SerializeField] private ParticleSystem effect;


        private bool _isActive;

        protected override async UniTaskVoid Init()
        {
            effect.Play();
            transform.position = PlayerCore.Rigidbody.position;

            PlayerCore.PlayerParameter.InvincibleFlag++;

            _isActive = true;

            var ed = SpellData.EffectDuration;
            PlayerBuff.BuffParameters.Add(
                new BuffParameter(BuffKey.ReflectionMirror, SpellData.SpellKey, ed));

            await MyDelay(ed);
            PlayerCore.PlayerParameter.InvincibleFlag--;

            _isActive = false;
            Destroy(gameObject);
        }

        private void FixedUpdate()
        {
            transform.position = PlayerCore.Rigidbody.position;
        }

        public void OnAttacked(AttackHitController attackHitController)
        {
            Shoot().Forget();
        }

        private async UniTaskVoid Shoot()
        {
            var pos = PlayerCore.Rigidbody.position +
                      (Vector3)Vector2Extension.AngleToVector(Random.Range(0f, Mathf.PI * 2f)) * 1.5f;
            var target = AllCharacterManager.GetEnemyCharacters().GetRandomValue();
            var instance = Instantiate(beamPrefab);
            instance.SetPositionAndDirection(pos, target.Rigidbody.position - pos);
            await UniTask.Yield(cancellationToken: destroyCancellationToken);
            instance.Activate(0.5f).Forget();

            await MyDelay(1f);
            Destroy(instance.gameObject);
        }


        public bool CheckHit(AttackHitController attackHitController)
        {
            if (!_isActive)
                return false;

            return AttackDatabase.Find(attackHitController.AttackKey).OwnerType == OwnerType.Enemy;
        }
    }
}