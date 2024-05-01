using UnityEngine;
using UnityEngine.UI;

namespace Battle.UI
{
    public class PlayerBuffViewChild : MonoBehaviour
    {
        [SerializeField] private Image backGround;
        [SerializeField] private Image icon;
        [SerializeField] private Image durationCircle;

        public void SetBackGroundColor(Color color) => backGround.color = color;

        public void SetIcon(Sprite sprite) => icon.sprite = sprite;

        public void SetCircleFill(float fillAmount) => durationCircle.fillAmount = fillAmount;
    }
}