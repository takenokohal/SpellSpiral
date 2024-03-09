using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DeckEdit.View
{
    public class DeckCursorView : MonoBehaviour
    {
        [Inject] private readonly PlayerInput _playerInput;
        [Inject] private readonly DeckListView _deckListView;
        [Inject] private readonly DeckList _deckList;
        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;
        [Inject] private readonly SpellDatabase _spellDatabase;

        [SerializeField] private int xMax = 10;
        [SerializeField] private int yMax = 2;

        private Vector2Int CurrentPos { get; set; }

        private int CurrentIndex
        {
            get => CurrentPos.x + CurrentPos.y * xMax;
            set => CurrentPos = new Vector2Int(value % xMax, value / xMax);
        }

        [SerializeField] private Transform cursor;

        private bool _isActive = true;

        private void Update()
        {
            if (!_isActive)
                return;

            ManageMove();
            ManageClick();
        }

        private void ManageClick()
        {
            if (!_playerInput.actions["Yes"].WasPressedThisFrame())
                return;


            var key = FindKey();
            _deckList.Remove(key);

            if (CurrentIndex >= GetCurrentDeckLength())
                CurrentIndex--;

            UniTask.Void(async () =>
            {
                await UniTask.Yield();
                UpdateView(FindKey());
            });
        }

        private void ManageMove()
        {
            var input = GetInputInt();
            if (input is { x: 0, y: 0 })
                return;
            var length = GetCurrentDeckLength();

            CurrentPos += input;
            CurrentPos = new Vector2Int(Repeat(CurrentPos.x, xMax), Repeat(CurrentPos.y, yMax));

            if (CurrentIndex >= length)
            {
                if (input.x > 0)
                    CurrentPos = new Vector2Int(0, CurrentPos.y);

                else
                    CurrentIndex = length - 1;
            }


            var key = FindKey();
            _currentSelectedSpell.SetSelectData(_spellDatabase.Find(key.Key));

            UpdateView(key);
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

        private int GetCurrentDeckLength()
        {
            return _deckList.CurrentDeckList.Count;
        }

        private int Repeat(int value, int max)
        {
            if (value >= max)
                value -= max;

            else if (value < 0)
                value += max;

            return value;
        }

        private SpellKey FindKey()
        {
            var key = _deckList.CurrentDeckList[CurrentIndex];
            return key;
        }

        private void UpdateView(SpellKey spellKey)
        {
            var view = _deckListView.IconDictionary[spellKey];

            cursor.position = view.transform.position;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
            cursor.gameObject.SetActive(isActive);
        }
    }
}