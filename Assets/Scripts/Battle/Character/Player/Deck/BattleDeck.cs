using System;
using System.Collections.Generic;
using System.Linq;
using DeckEdit.Model;
using Others;
using Others.Utils;
using UniRx;
using UnityEngine;
using VContainer;

namespace Battle.Character.Player.Deck
{
    public class BattleDeck : IDisposable
    {
        [Inject] private readonly IDeckPresenter _deckPresenter;


        private DeckData _originDeck;

        private readonly List<string> _currentDeck = new();

        private readonly Subject<Unit> _onDraw = new();
        public IObservable<Unit> OnDraw => _onDraw.AddTo(new List<IDisposable> { this });


        public void Init()
        {
            _originDeck = _deckPresenter.LoadDeck();
            ResetDeck();
            
            //Debug
         //   _currentDeck.Insert(0, _originDeck.highlanderSpell);

        }


        public void Draw(out string spellKey)
        {
            var v = _currentDeck.First();
            _currentDeck.Remove(v);
            spellKey = v;

            if (GetCount() <= 0)
            {
                ResetDeck();

                _currentDeck.Insert(0, _originDeck.highlanderSpell);
            }


            _onDraw.OnNext(Unit.Default);
        }

        private void ResetDeck()
        {
            _currentDeck.Clear();
            _currentDeck.AddRange(_originDeck.normalSpellDeck.Shuffle());
        }

        public int GetCount() => _currentDeck.Count;

        public interface IDeckPresenter
        {
            public DeckData LoadDeck();
        }

        public void Dispose()
        {
            _onDraw?.Dispose();
        }
    }
}