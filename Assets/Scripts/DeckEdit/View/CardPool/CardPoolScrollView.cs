using UnityEngine;
using UnityEngine.UI;

namespace DeckEdit.View.CardPool
{
    public class CardPoolScrollView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayoutGroup;


        public int ScrollOffset { get; set; }

        private void Update()
        {
            var parent = gridLayoutGroup.transform;

            var perScroll = gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;

            var scroll = ScrollOffset * perScroll;

            var pos = parent.localPosition;
            var to = Mathf.Lerp(pos.y, scroll, 0.2f);

            parent.localPosition = new Vector3(pos.x, to);
        }
    }
}