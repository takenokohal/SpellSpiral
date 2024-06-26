using Audio;
using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using Others;
using Others.Dialog;
using Others.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using VContainer;

namespace DeckEdit.View
{
    public class CardPoolCursorView : MonoBehaviour
    {
        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;
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
        
        private float _preInput;
        private const float Threshold = 0.5f;
        [Inject] private readonly OkDialog _okDialog;
        [Inject] private readonly YesNoDialog _yesNoDialog;
        private bool AnyDialogIsOpen => _okDialog.IsOpen || _yesNoDialog.IsOpen;

        private void Update()
        {
            if (!_isActive)
                return;
            if(AnyDialogIsOpen)
                return;
            

            ManageMove();
            ManageClick();


            UpdateView();
            
            _preInput = PlayerInput.actions["Vertical"].ReadValue<float>();
        }

        private void ManageClick()
        {
            if (!PlayerInput.actions["Yes"].WasPressedThisFrame())
                return;


            AllAudioManager.PlaySe("Select");
            var key = FindKey();
            _deckList.Add(new SpellKey(key));
        }

        private void ManageMove()
        {
            var input = GetInputInt();
            if (input == 0)
                return;

            AllAudioManager.PlaySe("CursorMove");
            _currentIndex = Mathf.Clamp(_currentIndex - input, 0, GetCardPoolLength() - 1);
        }

        private int GetInputInt()
        {
            var input = PlayerInput.actions["Vertical"];
            if (!IsTrigger())
                return 0;
            var v = input.ReadValue<float>();
            return FloatToInt(v);
        }
        private bool IsTrigger()
        {
            var value = PlayerInput.actions["Vertical"].ReadValue<float>();
            switch (_preInput)
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