using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Battle.CommonObject.MagicCircle
{
    public class MagicCircle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;


        public void Init(Sprite sprite)
        {
            //一旦無し
            //spriteRenderer.sprite = sprite;
        }


        public async UniTask Activate(MagicCircleParameters magicCircleParameters)
        {
            transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            await TweenToUniTask(transform.DOScale(magicCircleParameters.Size, 0.2f));
        }

        public async UniTask Close()
        {
            await TweenToUniTask(transform.DOScale(0, 0.2f));
            gameObject.SetActive(false);
        }

        private UniTask TweenToUniTask(Tween tween)
        {
            return tween.ToUniTask(
                tweenCancelBehaviour: TweenCancelBehaviour.KillAndCancelAwait,
                cancellationToken: destroyCancellationToken);
        }
    }
}