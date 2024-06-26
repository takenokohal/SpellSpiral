using Audio;
using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using Others;
using Others.Dialog;
using Others.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DeckEdit.View
{
    public class DeckCursorView : MonoBehaviour
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;
        [Inject] private readonly DeckListView _deckListView;
        [Inject] private readonly DeckList _deckList;
        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;
        [Inject] private readonly SpellDatabase _spellDatabase;

        [SerializeField] private int xMax = 10;
        [SerializeField] private int yMax = 2;

        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;
        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;
        private Vector2Int CurrentPos { get; set; }

        
        private float _preInputX;
        private float _preInputY;
        private const float Threshold = 0.5f;

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
            
            if(AnyDialogIsOpen)
                return;

            ManageMove();
            ManageClick();            
            _preInputY = PlayerInput.actions["Vertical"].ReadValue<float>();
            _preInputX = PlayerInput.actions["Horizontal"].ReadValue<float>();

        }

        private void ManageClick()
        {
            if (!PlayerInput.actions["Yes"].WasPressedThisFrame())
                return;


            AllAudioManager.PlaySe("Select");
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
            var inputX = FloatToInt(PlayerInput.actions["Horizontal"].ReadValue<float>());
            if (!IsTriggerX())
                inputX = 0;
            var inputY = FloatToInt(PlayerInput.actions["Vertical"].ReadValue<float>());
            if (!IsTriggerY())
                inputY = 0;

            if (inputX == 0 && inputY == 0)
                return;
            var length = GetCurrentDeckLength();

            CurrentPos += new Vector2Int(inputX, inputY);
            CurrentPos = new Vector2Int(Repeat(CurrentPos.x, xMax), Repeat(CurrentPos.y, yMax));

            if (CurrentIndex >= length)
            {
                if (inputX > 0)
                    CurrentPos = new Vector2Int(0, CurrentPos.y);

                else
                    CurrentIndex = length - 1;
            }


            var key = FindKey();
            _currentSelectedSpell.SetSelectData(_spellDatabase.Find(key.Key));

            UpdateView(key);
            AllAudioManager.PlaySe("CursorMove");
        }
        
        private bool IsTriggerX()
        {
            var value = PlayerInput.actions["Horizontal"].ReadValue<float>();
            switch (_preInputX)
            {
                case < Threshold when value >= Threshold:
                case > -Threshold when value <= -Threshold:
                    return true;
                default:
                    return false;
            }
        }
        
        private bool IsTriggerY()
        {
            var value = PlayerInput.actions["Vertical"].ReadValue<float>();
            switch (_preInputY)
            {
                case < Threshold when value >= Threshold:
                case > -Threshold when value <= -Threshold:
                    return true;
                default:
                    return false;
            }
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