using System;
using System.Threading;
using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UniRx;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class HomingBullet : MonoBehaviour
    {
        private Rigidbody _rb;
        private AttackHitController _attackHitController;


        private Parameter _parameter;


        private float _elapsedTime;

        private readonly Subject<Unit> _onKill = new();
        public IObservable<Unit> OnKill => _onKill.TakeUntilDestroy(this);

        private CancellationTokenSource _autoKillTokenSource;

        public class Parameter
        {
            public Transform Target { get; set; }

            public Vector2 FirstPos { get; set; }
            public Vector2 FirstVelocity { get; set; }

            public float MaxSpeed { get; set; }

            public float Duration { get; set; }

            public float ChangeSpeedValue { get; set; }
        }


        public HomingBullet CreateFromPrefab()
        {
            var v = Instantiate(this);
            v.Init();

            return v;
        }


        private void Init()
        {
            _rb = GetComponent<Rigidbody>();
            _attackHitController = GetComponent<AttackHitController>();


            _attackHitController.OnAttackHit.Subscribe(_ => Kill().Forget());
        }

        public void Activate(Parameter parameter)
        {
            AudioManager.PlaySe("MagicShot");

            _autoKillTokenSource = new CancellationTokenSource();
            gameObject.SetActive(true);
            _parameter = parameter;

            _elapsedTime = 0f;

            transform.position = _parameter.FirstPos;
            _rb.velocity = _parameter.FirstVelocity;
            AutoKill().Forget();
        }

        private void FixedUpdate()
        {
            var dt = Time.fixedDeltaTime;

            _elapsedTime += dt;


            var to = _elapsedTime <= _parameter.Duration
                ? (_parameter.Target.position - transform.position).normalized * _parameter.MaxSpeed
                : _rb.velocity.normalized * _parameter.MaxSpeed;


            _rb.velocity = Vector3.MoveTowards(_rb.velocity, to, _parameter.ChangeSpeedValue);
        }


        public async UniTaskVoid Kill()
        {
            _autoKillTokenSource?.Cancel();
            _autoKillTokenSource = null;
            _onKill.OnNext(Unit.Default);

            await transform.DOScale(0, 0.5f);

            Destroy(gameObject);
        }

        private async UniTaskVoid AutoKill()
        {
            await UniTask.Delay(10000, cancellationToken: _autoKillTokenSource.Token);
            Kill().Forget();
        }
    }
}