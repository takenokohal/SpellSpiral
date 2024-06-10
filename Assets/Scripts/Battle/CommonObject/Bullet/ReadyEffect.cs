using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.CommonObject.Bullet
{
    public class ReadyEffect : MonoBehaviour
    {
        [SerializeField] private ParticleSystem ps;

        public void Activate()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        private UniTask TweenToUniTask(Tween tween)
        {
            return tween.ToUniTask(cancellationToken: destroyCancellationToken);
        }
    }
}