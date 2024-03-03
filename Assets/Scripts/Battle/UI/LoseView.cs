using Battle.CommonObject.Result;
using UnityEngine;
using VContainer;

namespace Battle.UI
{
    public class LoseView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup mainCanvas;

        [Inject] private readonly LoseController _loseController;

        private void Start()
        {
        }
    }
}