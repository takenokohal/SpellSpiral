﻿using System;
using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class SpinBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private AttackHitController attackHitController;


        private Parameter _parameter;

        private float _elapsedTime;

        private readonly ReactiveProperty<bool> _isDead = new();

        public bool IsDead
        {
            get => _isDead.Value;
            private set => _isDead.Value = value;
        }

        public IObservable<bool> IsDeadObservable => _isDead.TakeUntilDestroy(this);

        public class Parameter
        {
            public Vector2 FirstPos { get; set; }
            public Vector2 FirstVelocity { get; set; }

            public float SpinAngle { get; set; }

            public float SpinPower { get; set; }
        }

        public SpinBullet CreateFromPrefab(Parameter parameter)
        {
            var v = Instantiate(this);
            v.Activate(parameter);

            return v;
        }


        private void Activate(Parameter parameter)
        {
            AllAudioManager.PlaySe("MagicShot");

            gameObject.SetActive(true);
            _parameter = parameter;

            _elapsedTime = 0f;

            transform.position = _parameter.FirstPos;
            rb.velocity = _parameter.FirstVelocity;
            AutoKill().Forget();

            attackHitController.OnAttackHit.Subscribe(_ => Kill().Forget());
        }

        private void FixedUpdate()
        {
            var velocityDir = rb.velocity.normalized;
            var forceDir = Quaternion.Euler(0, 0, _parameter.SpinAngle) * velocityDir;

            rb.AddForce(forceDir * _parameter.SpinPower);
        }


        public async UniTaskVoid Kill()
        {
            if (IsDead)
                return;

            IsDead = true;

            rb.velocity /= 2f;
            await transform.DOScale(0, 0.2f);

            Destroy(gameObject);
        }

        private async UniTaskVoid AutoKill()
        {
            await UniTask.Delay(10000, cancellationToken: destroyCancellationToken);
            Kill().Forget();
        }
    }
}