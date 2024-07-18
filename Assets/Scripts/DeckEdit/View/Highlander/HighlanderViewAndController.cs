using System.Linq;
using Cysharp.Threading.Tasks;
using Databases;
using DeckEdit.Model;
using DG.Tweening;
using Others;
using Others.Input;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace DeckEdit.View.Highlander
{
    public class HighlanderViewAndController : MonoBehaviour
    {
        [SerializeField] private SpellIconView currentSelectedIcon;

        [SerializeField] private SpellIconView leftIcon;
        [SerializeField] private SpellIconView rightIcon;

        [SerializeField] private SpellIconView leftLeftIcon;
        [SerializeField] private SpellIconView rightRightIcon;


        [SerializeField] private Transform parent;

        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly MyDeckModel _myDeckModel;

        [Inject] private readonly MyInputManager _myInputManager;
        private PlayerInput PlayerInput => _myInputManager.PlayerInput;
        [Inject] private readonly DeckEditStateModel _deckEditStateModel;

        [Inject] private readonly CurrentSelectedSpell _currentSelectedSpell;

        [Inject] private readonly HighlanderAnimation _highlanderAnimation;

        private bool _isActive;


        private float _preInputX;
        private float _preInputY;
        private const float Threshold = 0.5f;
        
        

        private void Start()
        {
            _myDeckModel.HighlanderObservable.Subscribe(OnChange).AddTo(this);

            _deckEditStateModel.StateObservable
                .Subscribe(value => SetActive(value == DeckEditState.Highlander).Forget())
                .AddTo(this);
        }

        private async UniTaskVoid SetActive(bool isActive)
        {
            _isActive = isActive;

            if (isActive)
            {
                parent.gameObject.SetActive(true);
                parent.DOScale(1, 0.2f);
            }
            else
            {
                await parent.DOScale(0, 0.2f);
                parent.gameObject.SetActive(false);
            }
        }

        private void OnChange(SpellKey spellKey)
        {
            var list = _spellDatabase.SpellDictionary
                .Where(value => value.Value.SpellAttribute == SpellAttribute.Highlander)
                .Select(value => value.Value.SpellKey)
                .ToList();
            

            var index = list.FindIndex(s => s == spellKey.Key);
            var leftKey = list[Repeat(index - 1, list.Count)];
            var leftLeftKey = list[Repeat(index - 2, list.Count)];
            var rightKey = list[Repeat(index + 1, list.Count)];
            var rightRightKey = list[Repeat(index + 2, list.Count)];

            var data = _spellDatabase.Find(spellKey.Key);
            currentSelectedIcon.SetIcon(data.SpellIcon);
            leftIcon.SetIcon(_spellDatabase.Find(leftKey).SpellIcon);
            rightIcon.SetIcon(_spellDatabase.Find(rightKey).SpellIcon);

            leftLeftIcon.SetIcon(_spellDatabase.Find(leftLeftKey).SpellIcon);
            rightRightIcon.SetIcon(_spellDatabase.Find(rightRightKey).SpellIcon);

            _currentSelectedSpell.SetSelectData(data);
        }

        private int Repeat(int i, int length)
        {
            if (i >= length)
                i -= length;
            else if (i < 0)
                i += length;

            return i;
        }

        private void Update()
        {
            if (!_isActive)
                return;

            ManageMove();
            ManageDown();

            _preInputY = PlayerInput.actions["Vertical"].ReadValue<float>();
            _preInputX = PlayerInput.actions["Horizontal"].ReadValue<float>();
        }

        private void ManageMove()
        {
            var inputX = FloatToInt(PlayerInput.actions["Horizontal"].ReadValue<float>());
            if (!IsTriggerX())
                inputX = 0;

            if (inputX != 0)
                Move(inputX);
        }

        private void Move(int indexMoveValue)
        {
            var list = _spellDatabase.SpellDictionary
                .Where(value => value.Value.SpellAttribute == SpellAttribute.Highlander)
                .Select(value => value.Value.SpellKey)
                .ToList();

            var index = list.FindIndex(s => s == _myDeckModel.CurrentHighlanderSpell.Key);

            var nextKey = list[Repeat(index + indexMoveValue, list.Count)];

            _myDeckModel.CurrentHighlanderSpell = new SpellKey(nextKey);

            switch (indexMoveValue)
            {
                case > 0:
                    _highlanderAnimation.AnimateToRight().Forget();
                    break;
                case < 0:
                    _highlanderAnimation.AnimateToLeft().Forget();
                    break;
            }
        }

        private void ManageDown()
        {
            var inputY = FloatToInt(PlayerInput.actions["Vertical"].ReadValue<float>());

            if (inputY == -1)
                _deckEditStateModel.CurrentState = DeckEditState.MyDeck;
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
    }
}