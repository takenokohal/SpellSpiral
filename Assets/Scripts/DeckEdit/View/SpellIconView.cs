using UnityEngine;
using UnityEngine.UI;

namespace DeckEdit.View
{
    public class SpellIconView : MonoBehaviour
    {
        [SerializeField] private Image backGroundImage;
        [SerializeField] private Image iconImage;


        public void SetColor(Color color) => backGroundImage.color = color;

        public void SetIcon(Sprite sprite)
        {
            iconImage.gameObject.SetActive(sprite != null);
            iconImage.sprite = sprite;
        }
    }
}