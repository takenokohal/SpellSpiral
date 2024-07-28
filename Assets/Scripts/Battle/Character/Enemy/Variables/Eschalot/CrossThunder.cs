using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using Others.Utils;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class CrossThunder : BossSequenceBase<EschalotState>
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float moveDrag;

        [SerializeField] private SingleHitBeam beamOrigin;
        [SerializeField] private int beamCount;

        [SerializeField] private float duration;

        [SerializeField] private float positionOffset;


        [SerializeField] private float recovery;

        private readonly List<SingleHitBeam> _beamInstances = new();

        public override EschalotState StateKey => EschalotState.CrossThunder;

        private void Start()
        {
            for (int i = 0; i < beamCount; i++)
            {
                var instance = Instantiate(beamOrigin, transform);
                _beamInstances.Add(instance);
            }
        }

        protected override async UniTask Sequence()
        {
            var r = Random.Range(0, 2f * Mathf.PI);
            Parent.Rigidbody.velocity = Vector2Extension.AngleToVector(r) * moveSpeed;
            Parent.Rigidbody.drag = moveDrag;

            for (int i = 0; i < _beamInstances.Count; i++)
            {
                Shoot(i).Forget();
            }

            await MyDelay(duration);

            Parent.Rigidbody.drag = 0;
            Parent.Rigidbody.velocity = Vector3.zero;
            await MyDelay(recovery);
        }

        private async UniTaskVoid Shoot(int i)
        {
            ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                Parent,
                () => CalcPos(i),
                2,
                () => CalcRot(i)* Mathf.Rad2Deg)).Forget();

            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 2, () => CalcPos(i)));

            _beamInstances[i].SetPositionAndRotation(CalcPos(i), CalcRot(i)* Mathf.Rad2Deg);
            _beamInstances[i].Activate(duration).Forget();
        }

        private Vector2 CalcPos(int i)
        {
            return Vector2Extension.AngleToVector(CalcRot(i)) * positionOffset + (Vector2)Parent.Rigidbody.position;
        }

        private float CalcRot(int i)
        {
            return Mathf.PI * 2f / _beamInstances.Count * i;
        }
    }
}