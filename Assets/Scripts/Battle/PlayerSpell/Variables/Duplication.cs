/*using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Battle.PlayerSpell.Variables
{
    public class Duplication : SpellBase
    {
        [SerializeField] private Animator shadow;

        private readonly List<MyClass> _parameters = new();

        private class MyClass
        {
            public Vector3 pos;
            public Quaternion rot;
            public float time;
        }

        protected override async UniTaskVoid Init()
        {
            await UniTask.Yield();

            SpellFactory.OnSpellCreated.Take(1).Subscribe(key => OnChant(key).Forget()).AddTo(this);

            await UniTask.CompletedTask;
        }

        private void FixedUpdate()
        {
            var t = PlayerCore.Animator.transform;
            _parameters.Add(new MyClass()
            {
                pos = t.position,
                rot = t.rotation,
                time = Time.time
            });

            var v = _parameters.FirstOrDefault(value => Time.time - value.time >= 0.5f);
            if (v == null)
                return;

            _parameters.Remove(v);
            shadow.transform.position = v.pos;
            shadow.transform.rotation = v.rot;
        }


        private async UniTaskVoid OnChant(string key)
        {
            await MyDelay(0.5f);

            SpellFactory.Create(key);

            await shadow.transform.DOScale(0, 0.5f);

            Destroy(gameObject);
        }
    }
}*/