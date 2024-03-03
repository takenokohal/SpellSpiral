﻿using System;
using System.Collections.Generic;
using System.Linq;
using Others;
using Others.Utils;
using UniRx;

namespace Battle.Character.Player.Deck
{
    public class BattleDeck : IDisposable
    {
        private readonly IDeckPresenter _deckPresenter;
        private readonly IReadOnlyList<string> _originDeck;
        private readonly List<string> _currentDeck = new();

        private readonly Subject<Unit> _onDraw = new();
        public IObservable<Unit> OnDraw => _onDraw;

        public BattleDeck(IDeckPresenter deckPresenter)
        {
            _deckPresenter = deckPresenter;
            _originDeck = _deckPresenter.LoadDeck();
            ResetDeck();
        }


        public void Draw(out string spellKey)
        {
            var v = _currentDeck.First();
            _currentDeck.Remove(v);
            spellKey = v;

            if (GetCount() <= 0)
                ResetDeck();

            _onDraw.OnNext(Unit.Default);
        }

        private void ResetDeck()
        {
            _currentDeck.Clear();
            _currentDeck.AddRange(_originDeck.Shuffle());
        }

        public int GetCount() => _currentDeck.Count;

        public interface IDeckPresenter
        {
            public IReadOnlyList<string> LoadDeck();
        }

        public void Dispose()
        {
            _onDraw?.Dispose();
        }
    }
}