using System;
using System.Linq;
using Battle.Character.Player.Buff;
using Battle.Character.Player.Deck;
using Battle.PlayerSpell;
using Cysharp.Threading.Tasks;
using Databases;
using Others;
using Others.Utils;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerChant : PlayerComponent
    {
        [Inject] private readonly BattleDeck _battleDeck;

        private readonly ReactiveDictionary<SpellSlot, string> _currentSpells = new();
        public IReadOnlyReactiveDictionary<SpellSlot, string> CurrentSpells => _currentSpells;
        [ShowInInspector] private IReactiveDictionary<SpellSlot, string> ForInspector => _currentSpells;

        [Inject] private readonly SpellFactory _spellFactory;
        [Inject] private readonly SpellDatabase _spellDatabase;

        private static readonly int AnimKey = Animator.StringToHash("Chant");

        private readonly Subject<SpellSlot> _onChantMiss = new();
        public IObservable<SpellSlot> OnChantMiss => _onChantMiss;

        private readonly Subject<SpellSlot> _onChantSuccess = new();
        public IObservable<SpellSlot> OnChantSuccess => _onChantSuccess;

        protected override void Init()
        {
            _battleDeck.Init();

            foreach (var spellSlot in EnumUtil<SpellSlot>.GetValues())
            {
                _battleDeck.Draw(out var spell);
                _currentSpells.Add(spellSlot, spell);
            }
        }


        // Update is called once per frame
        private void Update()
        {
            if (!PlayerCore.IsBattleStarted)
                return;

            foreach (var spellSlot in EnumUtil<SpellSlot>.GetValues())
            {
                var input = CheckInput(spellSlot);

                if (!input)
                    continue;

                var chantSuccess = TryChant(spellSlot);

                if (chantSuccess)
                {
                    _onChantSuccess.OnNext(spellSlot);
                    ChangeSpell(spellSlot);
                }
                else
                {
                    _onChantMiss.OnNext(spellSlot);
                }
            }
        }


        private bool CheckInput(SpellSlot spellSlot)
        {
            var key = "Chant_" + spellSlot;
            var input = PlayerCore.PlayerInput.actions[key];
            return input.WasPressedThisFrame();
        }

        private bool TryChant(SpellSlot spellSlot)
        {
            var spellKey = _currentSpells[spellSlot];
            var spellData = _spellDatabase.SpellDictionary[spellKey];
            var manaCost = (float)spellData.ManaCost;

            var reduceCostCount = PlayerBuff.BuffCount(BuffKey.ReduceCost);
            for (int i = 0; i < reduceCostCount; i++)
            {
                manaCost *= PlayerConstData.BuffReduceManaRatio;
            }

            if (PlayerParameter.Mana < manaCost)
            {
                return false;
            }

            PlayerParameter.Mana -= manaCost;

            _spellFactory.Create(spellKey);

            TryDuplication(spellKey).Forget();


            switch (spellData.SpellType)
            {
                case SpellType.Attack:
                    PlayerCore.Animator.Play("Attack", 0, 0);

                    break;
                case SpellType.Support:
                    PlayerCore.Animator.Play("Buff", 0, 0);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return true;
        }

        private async UniTaskVoid TryDuplication(string spellKey)
        {
            var dups = PlayerBuff.BuffParameters.Where(value => value.BuffKey == BuffKey.Duplication).ToList();

            foreach (var buffParameter in dups)
            {
                await UniTask.Delay(500, cancellationToken: destroyCancellationToken);
                _spellFactory.Create(spellKey);

                PlayerBuff.BuffParameters.Remove(buffParameter);
            }
        }

        private void ChangeSpell(SpellSlot spellSlot)
        {
            _battleDeck.Draw(out var nextSpell);
            _currentSpells[spellSlot] = nextSpell;
        }
    }
}