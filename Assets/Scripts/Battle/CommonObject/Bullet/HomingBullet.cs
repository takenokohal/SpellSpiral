using System;
using System.Threading;
using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class HomingBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody rb;
        [SerializeField] private AttackHitController attackHitController;


        private Parameter _parameter;

        private float _elapsedTime;

        private readonly ReactiveProperty<bool> _isDead = new();

        [SerializeField] public int hitCount = 1;

        private int _currentHitCount;

        public bool IsDead
        {
            get => _isDead.Value;
            private set => _isDead.Value = value;
        }

        public IObservable<bool> IsDeadObservable => _isDead.TakeUntilDestroy(this);

        public class Parameter
        {
            public Transform Target { get; set; }

            public Vector2 FirstPos { get; set; }
            public Vector2 FirstVelocity { get; set; }

            public float MaxSpeed { get; set; }

            public float Duration { get; set; }

            public float ChangeSpeedValue { get; set; }
        }

        public HomingBullet CreateFromPrefab(Parameter parameter)
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

            var target = _parameter.Target;
            target.OnDestroyAsObservable().Take(1).TakeUntilDestroy(this).Subscribe(_ => _elapsedTime = 114514);

            attackHitController.OnAttackHit
                .Where(_ => !IsDead)
                .Subscribe(_ =>
            {
                _currentHitCount++;
                if (_currentHitCount >= hitCount)
                    Kill().Forget();
            });
        }

        private void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime;

            _elapsedTime += dt;


            var to = (_elapsedTime <= _parameter.Duration)
                ? (_parameter.Target.position - transform.position).normalized * _parameter.MaxSpeed
                : rb.velocity.normalized * _parameter.MaxSpeed;


            rb.velocity = Vector3.MoveTowards(rb.velocity, to, _parameter.ChangeSpeedValue);
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