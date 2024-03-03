using UnityEngine;

namespace Battle.CommonObject.MagicCircle
{
    public class MagicCircle : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        public void Init(Sprite sprite, Color color)
        {
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = color;
        }
    }
}