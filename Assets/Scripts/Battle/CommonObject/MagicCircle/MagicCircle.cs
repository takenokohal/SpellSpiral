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
    }
}