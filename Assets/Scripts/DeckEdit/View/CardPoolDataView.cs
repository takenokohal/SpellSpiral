using System;
using Battle.PlayerSpell;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DeckEdit.View
{
    public class CardPoolDataView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [SerializeField] private Image iconImage;

        [SerializeField] private Image colorImage;
        
        
        public void SetIcon(Sprite icon) => iconImage.sprite = icon;

        public void SetColor(Color color) => colorImage.color = color;

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
            Debug.Log(_onRightClick);
            _onMouseEnter.OnNext(Unit.Default);
        }
    }
}