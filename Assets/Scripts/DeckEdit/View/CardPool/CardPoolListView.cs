using System.Collections.Generic;
using System.Linq;
using Databases;
using DeckEdit.Model;
using Others;
using Spell;
using UniRx;
using UnityEngine;
using VContainer;

namespace DeckEdit.View.CardPool
{
    public class CardPoolListView : MonoBehaviour
    {
        [SerializeField] private Transform contentsParent;
        [SerializeField] private SpellIconView spellIconViewPrefab;

        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _spellColorPalette;
        [Inject] private readonly CardPoolModel _cardPoolModel;
        [Inject] private readonly CardPoolScrollView _scrollView;

        private SpellIconView[] _iconViewInstances;
        public IReadOnlyList<SpellIconView> IconViewInstances => _iconViewInstances;

        [SerializeField] private int yLength;

        public bool IsUpperOfView(int y)
        {
            return y >= _scrollView.ScrollOffset + yLength;
        }

        public bool IsLowerOfView(int y)
        {
            return y < _scrollView.ScrollOffset;
        }


        public bool IsInitialized { get; private set; }

        private void Start()
        {
            Init();

            _cardPoolModel.OnUpdate.Subscribe(_ => { OnUpdate(); }).AddTo(this);

            IsInitialized = true;
        }

        private void Init()
        {
            _iconViewInstances =
                new SpellIconView[_spellDatabase.SpellDictionary.Count(value =>
                    !value.Value.IsNotImp() && value.Value.SpellAttribute != SpellAttribute.Highlander)];
            for (var i = 0; i < _iconViewInstances.Length; i++)
            {
                var instance = Instantiate(spellIconViewPrefab, contentsParent);
                instance.gameObject.SetActive(true);
                _iconViewInstances[i] = instance;
            }
        }

        private void OnUpdate()
        {
            var list = _cardPoolModel.CurrentSortedCardPoolList;
            for (var i = 0; i < _iconViewInstances.Length; i++)
            {
                var instance = _iconViewInstances[i];

                var key = list[i];
                var data = _spellDatabase.Find(key.Key);
                var color = _spellColorPalette.GetColor(data.SpellAttribute);
                instance.SetIcon(data.SpellIcon);
                instance.SetColor(color);
            }
        }
    }
}