using System;
using Audio;
using Battle.Attack;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class SingleHitBeam : MonoBehaviour
    {
        [SerializeField] private AttackHitController attackHitControllerInChildren;
        [SerializeField] private ParticleSystem effect;


        private void Start()
        {
            attackHitControllerInChildren.gameObject.SetActive(false);
        }

        public void SetPosition(Vector2 pos)
        {
            transform.position = pos;
        }

        public void SetRotation(float rot)
        {
            transform.rotation = Quaternion.Euler(0, 0, rot);
        }

        public void SetDirection(Vector2 direction)
        {
            SetRotation(Mathf.Atan2(direction.y, direction.x)* Mathf.Rad2Deg);
        }

        public void SetPositionAndRotation(Vector2 pos, float rot)
        {
            SetPosition(pos);
            SetRotation(rot);
        }

        public void SetPositionAndDirection(Vector2 pos, Vector2 direction)
        {
            SetPosition(pos);
            SetDirection(direction);
        }

        public async UniTask Activate(float lifeTime)
        {
            attackHitControllerInChildren.gameObject.SetActive(true);

            effect.Play();
            AllAudioManager.PlaySe("Laser");

            await UniTask.Delay((int)(lifeTime * 1000f), cancellationToken: destroyCancellationToken);

            attackHitControllerInChildren.gameObject.SetActive(false);
            effect.Stop();
        }
    }
}