using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class MultiHitLaser : MonoBehaviour
    {
        [SerializeField] private AttackHitController attackHitControllerInChildren;
        [SerializeField] private ParticleSystem effect;


        private const float HitCountCoolTime = 0.2f;

        public bool IsActive { get; private set; }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public void SetRotation(float rot)
        {
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }

        public void SetPositionAndRotation(Vector2 pos, float rot)
        {
            SetPosition(pos);
            SetRotation(rot);
        }

        public async UniTask Activate(float lifeTime)
        {
            effect.Play();

            IsActive = true;
            HitLoop().Forget();

            await MyDelay(lifeTime);

            IsActive = false;

            effect.Stop();
            effect.Clear();
        }

        private async UniTaskVoid HitLoop()
        {
            while (IsActive)
            {
                attackHitControllerInChildren.gameObject.SetActive(true);
                AllAudioManager.PlaySe("Laser");
                await MyDelay(HitCountCoolTime);
                attackHitControllerInChildren.gameObject.SetActive(false);
                await UniTask.Yield();
            }
        }

        private UniTask MyDelay(float duration)
        {
            return UniTask.Delay((int)(1000f * duration), cancellationToken: destroyCancellationToken);
        }
    }
}