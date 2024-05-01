using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Others;
using Others.Utils;
using UniRx;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Molle
{
    public class MolleController : BossBase<MolleState>
    {
        private readonly List<Vector3> _path = new();
        [SerializeField] private float maxX;
        [SerializeField] private float maxY;

        private float _currentAngle;
        [SerializeField] private float speed;


        protected override void InitializeFunction()
        {
            base.InitializeFunction();

            for (int i = 0; i < 100; i++)
            {
                var x = Random.Range(-maxX, maxX);
                var y = Random.Range(-maxY, maxY);
                _path.Add(new Vector3(x, y));
            }


            GameLoop.Event
                .Where(value => value == GameLoop.GameEvent.BattleStart)
                .Take(1)
                .Subscribe(_ =>
                {
                    
                    Loop().Forget();
                    
                    return;
                    Rigidbody
                        .DOPath(_path.ToArray(), 2, PathType.CatmullRom)
                        .SetEase(Ease.Linear)
                        .SetSpeedBased()
                        .SetLoops(-1);
                })
                .AddTo(this);
        }

        private void FixedUpdate()
        {
            return;
            _currentAngle += Time.fixedDeltaTime * speed;
            _currentAngle = Mathf.Repeat(_currentAngle, Mathf.PI * 2);
            var to = new Vector2(Mathf.Cos(_currentAngle) * maxX, Mathf.Sin(_currentAngle) * maxY);
            Rigidbody.position = Vector3.Lerp(Rigidbody.position, to, 0.2f);
        }

        private async UniTask Loop()
        {
            await PlayState(MolleState.SummonHighButterfly);

            var states = new List<MolleState>()
            {
                MolleState.SummonAssassin,
                MolleState.SummonADC,
                MolleState.SummonFighter,
                MolleState.SummonMage, 
                MolleState.SummonTank
            };
            while (!commonCancellationTokenSource.IsCancellationRequested)
            {
                var nextState = states.Where(value => value != CurrentState).GetRandomValue();
                
                LookPlayer();

                await PlayState(nextState);
                
            }
        }
    }
}