using Battle.Attack;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class Explosion : EnemySequenceBase
    {
        [SerializeField] private ParticleSystem startEffect;
        [SerializeField] private ParticleSystem explosionEffect;

        [SerializeField] private AttackHitController attackHitController;
        [SerializeField] private float delay;
        private static readonly int ChargingAnimKey = Animator.StringToHash("Charging");

        private void Start()
        {
            UniTask.Void(async () =>
            {
                await UniTask.WaitWhile(() => !Parent.IsInitialized);

                transform.SetParent(Parent.Center);
                transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            });
        }

        protected override async UniTask Sequence()
        {
            Animator.SetBool(ChargingAnimKey, true);
            startEffect.Play();
            await MyDelay(delay);

            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(CharacterKey.Baltecia, Color.red, 7,
                () => Parent.Center.position));

            startEffect.Stop();
            explosionEffect.Play();
            attackHitController.gameObject.SetActive(true);
            
            Animator.SetBool(ChargingAnimKey, false);

            await MyDelay(0.5f);
            attackHitController.gameObject.SetActive(false);
        }
    }
}