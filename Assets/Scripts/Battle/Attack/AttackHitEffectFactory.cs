using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Spell;
using UnityEngine;
using UnityEngine.Pool;

namespace Battle.Attack
{
    public class AttackHitEffectFactory : SerializedMonoBehaviour
    {
        [OdinSerialize] private Dictionary<SpellAttribute, ParticleSystem> _effects = new();
        private Dictionary<SpellAttribute, ObjectPool<ParticleSystem>> _pools = new();

        [SerializeField] private ParticleSystem servantDeadEffectPrefab;
        private ObjectPool<ParticleSystem> _deadEffectPool;

        private void Start()
        {
            foreach (var keyValuePair in _effects)
            {
                var key = keyValuePair.Key;
                var prefab = keyValuePair.Value;
                _pools.Add(key, new ObjectPool<ParticleSystem>(
                    () => Instantiate(prefab),
                    target => target.gameObject.SetActive(true),
                    target => target.gameObject.SetActive(false)));
            }

            _deadEffectPool = new ObjectPool<ParticleSystem>(
                () => Instantiate(servantDeadEffectPrefab),
                target => target.gameObject.SetActive(true),
                target => target.gameObject.SetActive(false));
        }


        public async UniTaskVoid Create(
            SpellAttribute spellAttribute,
            float damage,
            Vector3 position,
            Quaternion rotation)
        {
            var v = _pools[spellAttribute].Get();
            var t = v.transform;
            t.position = position;
            t.rotation = rotation;
            t.localScale = Vector3.one * (1f + damage / 100f);

            await UniTask.Delay(5000, cancellationToken: destroyCancellationToken);

            _pools[spellAttribute].Release(v);
        }

        public async UniTaskVoid CreateDeadEffect(Vector3 position, Quaternion rotation, float size)
        {
            var v = _deadEffectPool.Get();
            var t = v.transform;
            t.position = position;
            t.rotation = rotation;
            t.localScale = Vector3.one * 0.5f;

            v.Play();

            await UniTask.Delay(5000, cancellationToken: destroyCancellationToken);

            _deadEffectPool.Release(v);
        }
    }
}