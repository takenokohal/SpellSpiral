using Battle.Attack;
using Battle.CommonObject.MagicCircle;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.Character.Enemy.Variables.Baltecia
{
    public class FlameSword : EnemySequenceBase
    {
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private AttackHitController attackHitController;

        [SerializeField] private float drag;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float recovery;

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
            var offset = GetDirectionToPlayer().x >= 0 ? -1.5f : 1.5f;

            var targetPos = PlayerCore.Center.position + new Vector3(offset, 0f);
            var dir = (targetPos - Parent.Center.position).normalized;

            var rb = Parent.Rigidbody;
            rb.drag = drag;
            rb.velocity = moveSpeed * dir;

            Parent.ToAnimationVelocity = dir;

            await MagicCircleFactory.CreateAndWait(new MagicCircleParameters(
                CharacterKey.Baltecia,
                Color.red, 3f,
                () => Parent.Center.position - new Vector3(offset, 0)));


            effect.Play();
            attackHitController.gameObject.SetActive(true);

            await MyDelay(0.5f);
            
            Parent.ToAnimationVelocity= Vector2.zero;

            attackHitController.gameObject.SetActive(false);

            rb.drag = 0;

            await MyDelay(recovery);
        }
    }
}