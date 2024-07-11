using UnityEngine;
using UnityEngine.UI;

namespace NewDeckEdit.Test
{
    public class SpellIcon : MonoBehaviour
    {
        [SerializeField] private Image backGround;
        [SerializeField] private Image iconImage;

        public void SetIcon(Sprite sprite)
        {
            if (sprite == null)
            {
                iconImage.gameObject.SetActive(false);
                return;
            }


            iconImage.sprite = sprite;
        }

        public void SetColor(Color color)
        {
            backGround.color = color;
        }
    }
}