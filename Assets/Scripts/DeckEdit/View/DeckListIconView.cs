using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeckEdit.View
{
    public class DeckListIconView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private Image backGroundImage;
        [SerializeField] private Image iconImage;

        private ObservableEventTrigger _observableEventTrigger;

        public void SetColor(Color color) => backGroundImage.color = color;

        public void SetIcon(Sprite sprite) => iconImage.sprite = sprite;
        

        private readonly Subject<Unit> _onRightClick = new();
        public IObservable<Unit> OnRightClick => _onRightClick;

        private readonly Subject<Unit> _onMouseEnter = new();
        public IObservable<Unit> OnMouseEnter => _onMouseEnter;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Right)
                return;

            _onRightClick.OnNext(Unit.Default);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _onMouseEnter.OnNext(Unit.Default);
        }
    }
}