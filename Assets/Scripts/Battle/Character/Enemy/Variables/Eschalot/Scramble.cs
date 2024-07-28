using System.Collections.Generic;
using Battle.CommonObject.Bullet;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Eschalot
{
    public class Scramble : BossSequenceBase<EschalotState>
    {
        [SerializeField] private int beamCount;

        [SerializeField] private SingleHitBeam beamOrigin;

        [SerializeField] private float defaultDuration;
        [SerializeField] private float durationBounciness;

        private readonly List<SingleHitBeam> _beamInstances = new();

        [SerializeField] private float beamDuration;

        [SerializeField] private float rotateSpeed;


        [SerializeField] private float recovery;


        public override EschalotState StateKey => EschalotState.Scramble;

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
            CameraSwitcher.SetSpecialCameraSwitch(true);

            await TweenToUniTask(Parent.Rigidbody.DOMove(Vector3.zero, 0.5f));

            var currentDuration = defaultDuration;
            for (int i = 0; i < beamCount; i++)
            {
                Shoot(i).Forget();


                await MyDelay(currentDuration + 0.05f);
                currentDuration *= durationBounciness;
            }

            await MyDelay(recovery);
            CameraSwitcher.SetSpecialCameraSwitch(true);
        }

        private async UniTaskVoid Shoot(int i)
        {
            var pos = PlayerCore.Rigidbody.position;
            var rot = rotateSpeed * i;

            for (int j = 0; j < 2; j++)
            {
                var j1 = j;
                ReadyEffectFactory.BeamCreateAndWait(new ReadyEffectParameter(
                    Parent, () => pos, 1, () => rot + 180f * j1)).Forget();
            }

            await MagicCircleFactory.CreateAndWait(
                new MagicCircleParameters(Parent, 2, () => pos));


            var beam = _beamInstances[i];
            beam.Activate(beamDuration).Forget();
            beam.SetPositionAndRotation(pos, rot);
        }
    }
}