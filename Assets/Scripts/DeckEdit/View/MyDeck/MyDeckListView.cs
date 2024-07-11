using System.Collections.Generic;
using Databases;
using DeckEdit.Model;
using UniRx;
using UnityEngine;
using VContainer;

namespace DeckEdit.View.MyDeck
{
    public class MyDeckListView : MonoBehaviour
    {
        [SerializeField] private Transform contentsParent;
        [SerializeField] private SpellIconView spellIconViewPrefab;

        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly SpellColorPalette _spellColorPalette;

        private readonly SpellIconView[] _iconViewInstances = new SpellIconView[MyDeckModel.MaxCount];
        public IReadOnlyList<SpellIconView> IconViewInstances => _iconViewInstances;

        [SerializeField] private SpellIconView highlanderIcon;


        public bool IsInitialized { get; private set; }

        private void Start()
        {
            Init();
            IsInitialized = true;
        }

        private void Init()
        {
            for (var i = 0; i < _iconViewInstances.Length; i++)
            {
                var instance = Instantiate(spellIconViewPrefab, contentsParent);
                _iconViewInstances[i] = instance;
            }
        }

        public void UpdateHighlander(SpellKey spellKey)
        {
            var data = _spellDatabase.Find(spellKey.Key);
            highlanderIcon.SetIcon(data.SpellIcon);
            highlanderIcon.SetColor(_spellColorPalette.GetColor(data.SpellAttribute));
        }

        public void OnUpdate(IReadOnlyList<SpellKey> spellKeys)
        {
            for (var i = 0; i < _iconViewInstances.Length; i++)
            {
                var instance = _iconViewInstances[i];
                var outOfRange = i >= spellKeys.Count;

                instance.gameObject.SetActive(!outOfRange);

                if (outOfRange)
                {
                    continue;
                }

                var key = spellKeys[i];
                var data = _spellDatabase.Find(key.Key);
                var color = _spellColorPalette.GetColor(data.SpellAttribute);
                instance.SetIcon(data.SpellIcon);
                instance.SetColor(color);
            }
        }
    }
}