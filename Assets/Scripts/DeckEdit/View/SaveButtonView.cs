using DeckEdit.Model;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace DeckEdit.View
{
    public class SaveButtonView : MonoBehaviour
    {
        [SerializeField] private Button button;
        [Inject] private readonly DeckList _deckList;

        private void Start()
        {
            button.OnClickAsObservable().Subscribe(_ => _deckList.Save()).AddTo(this);
        }
    }
}