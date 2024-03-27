using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Others.Scene
{
    public class SceneFadePanelView : MonoBehaviour
    {
        [SerializeField] private Image panel;
        [SerializeField] private float duration;


        public async UniTask FadeIn()
        {
            await panel.DOFade(0, duration);
        }

        public async UniTask FadeOut()
        {
            await panel.DOFade(1, duration);
        }
    }
}