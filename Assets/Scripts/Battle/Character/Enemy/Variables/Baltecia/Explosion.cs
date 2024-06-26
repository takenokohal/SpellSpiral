using Audio;
using Battle.Attack;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class Explosion : BossSequenceBase<BalteciaState>
    {
        [SerializeField] private ParticleSystem startEffect;
        [SerializeField] private ParticleSystem explosionEffect;

        [SerializeField] private AttackHitController attackHitController;
        // [SerializeField] private float delay;

        private void Start()
        {
            UniTask.Void(async () =>
            {
                await UniTask.WaitWhile(() => !Parent.IsInitialized);

                transform.SetParent(Parent.transform);
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            });
        }

        public override BalteciaState StateKey => BalteciaState.Explosion;

        protected override async UniTask Sequence()
        {
            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Charge);
            startEffect.Play();
            //  await MyDelay(delay);

            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(Parent, 7,
                () => Parent.transform.position));

            startEffect.Stop();
            explosionEffect.Play();
            AllAudioManager.PlaySe("Explosion");
            attackHitController.gameObject.SetActive(true);

            WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Idle);

            await MyDelay(0.5f);
            attackHitController.gameObject.SetActive(false);
        }
    }
}