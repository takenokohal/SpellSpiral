using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace DeckEdit.View
{
    public class CardPoolCursorView : MonoBehaviour
    {
        [Inject] private readonly PlayerInput _playerInput;
        [Inject] private readonly CardPoolView _cardPoolView;
        [Inject] private readonly CardPool _cardPool;
        [Inject] private readonly DeckList _deckList;
        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;
        [Inject] private readonly SpellDatabase _spellDatabase;

        private int _currentIndex;

        [SerializeField] private RectTransform cursor;

        [SerializeField] private GridLayoutGroup gridLayoutGroup;
        [SerializeField] private float scrollMin;
        [SerializeField] private float scrollMax;


        private bool _isActive = true;

        private void Update()
        {
            if (!_isActive)
                return;

            ManageMove();
            ManageClick();


            UpdateView();
        }

        private void ManageClick()
        {
            if (!_playerInput.actions["Yes"].WasPressedThisFrame())
                return;


            var key = FindKey();
            _deckList.Add(new SpellKey(key));
        }

        private void ManageMove()
        {
            var input = GetInputInt();
            if (input.y == 0)
                return;

            _currentIndex = Mathf.Clamp(_currentIndex - input.y, 0, GetCardPoolLength() - 1);
        }

        private Vector2Int GetInputInt()
        {
            var input = _playerInput.actions["Move"];
            if (!input.triggered)
                return Vector2Int.zero;
            var v = input.ReadValue<Vector2>();
            var xInput = FloatToInt(v.x);
            var yInput = FloatToInt(v.y);
            return new Vector2Int(xInput, yInput);
        }

        private static int FloatToInt(float value)
        {
            const float threshold = 0.3f;
            return value switch
            {
                > threshold => 1,
                < -threshold => -1,
                _ => 0
            };
        }

        private int GetCardPoolLength()
        {
            return _cardPool.CurrentCardPool.Count;
        }

        private string FindKey()
        {
            var key = _cardPool.CurrentCardPool[_currentIndex];
            return key;
        }

        private void UpdateView()
        {
            var view = _cardPoolView.Instances[_currentIndex];

            cursor.position = view.transform.position;

            var y = gridLayoutGroup.transform.localPosition.y;
            if (cursor.anchoredPosition.y < scrollMin)
            {
                y += gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
            }
            else if (cursor.anchoredPosition.y > scrollMax)
            {
                y -= gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y;
            }

            var space = gridLayoutGroup.spacing.y;
            var max = (gridLayoutGroup.cellSize.y + space) * GetCardPoolLength() + space - 1000f;
            y = Mathf.Clamp(y, 0, max);

            gridLayoutGroup.transform.localPosition = new Vector3(0, y);


            var key = FindKey();
            _currentSelectedSpell.SetSelectData(_spellDatabase.Find(key));
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            cursor.gameObject.SetActive(isActive);
        }
    }
}