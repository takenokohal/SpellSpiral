using System;
using System.Linq;
using Battle.Character.Player.Buff;
using Battle.Character.Player.Deck;
using Battle.MyCamera;
using Battle.System.CutIn;
using Cysharp.Threading.Tasks;
using Databases;
using Others;
using Others.Utils;
using Sirenix.OdinInspector;
using Spell;
using UniRx;
using UnityEngine;
using VContainer;

namespace Battle.Character.Player
{
    public class PlayerChant : PlayerComponent
    {
        [Inject] private readonly BattleDeck _battleDeck;
        [Inject] private readonly CameraSwitcher _cameraSwitcher;

        private readonly ReactiveDictionary<SpellSlot, string> _currentSpells = new();

        public IReadOnlyReactiveDictionary<SpellSlot, string> CurrentSpells => _currentSpells;

        [Inject] private readonly SpellFactory _spellFactory;
        [Inject] private readonly SpellDatabase _spellDatabase;

        private readonly Subject<SpellSlot> _onChantMiss = new();
        public IObservable<SpellSlot> OnChantMiss => _onChantMiss;

        private readonly Subject<SpellSlot> _onChantSuccess = new();
        public IObservable<SpellSlot> OnChantSuccess => _onChantSuccess;

        [SerializeField] private ParticleSystem highlanderEffect;

        [Inject] private readonly CutInController _cutInController;

        private bool CantChant =>
            IsPlayerDead ||
            PlayerParameter.SpellChanting ||
            PlayerParameter.Warping;

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

            if (CantChant)
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


            switch (spellData.SpellType)
            {
                case SpellType.Attack:
                    WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Attack);
                    break;

                case SpellType.Support:
                    WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Buff);
                    break;


                case SpellType.Highlander:
                    ChantHighlander(spellKey).Forget();
                    return true;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _spellFactory.Create(spellKey);
            TryDuplication(spellKey).Forget();


            UniTask.Void(async () =>
            {
                PlayerParameter.SpellChanting = true;
                await MyDelay(PlayerConstData.ChantDuration);
                PlayerParameter.SpellChanting = false;
                var isCharged = PlayerCore.PlayerInput.actions["Charge"].IsPressed();
                if (isCharged)
                    WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Charge);
            });

            return true;
        }

        private async UniTaskVoid ChantHighlander(string spellKey)
        {
            PlayerParameter.SpellChanting = true;

            Time.timeScale = 0;

            await _cutInController.CutIn(PlayerCore.CharacterRotation.IsRight, PlayerCore.Animator.transform);
            //  PlayerCore.Animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            //  PlayerCore.Animator.Play("CutIn", 0, 0);
            //  _cameraSwitcher.SetIgnoreTimeScale(true);
            //    _cameraSwitcher.SetCutInSwitch(true);

            //  await UniTask.Delay(100, ignoreTimeScale: true, cancellationToken: destroyCancellationToken);
            //  _cameraSwitcher.SetCutInSwitch(false);

            //  await UniTask.Delay(500, ignoreTimeScale: true, cancellationToken: destroyCancellationToken);

            //  _cameraSwitcher.SetIgnoreTimeScale(false);
           Time.timeScale = 1;
            PlayerParameter.SpellChanting = false;

            //    PlayerCore.Animator.updateMode = AnimatorUpdateMode.AnimatePhysics;

            _spellFactory.Create(spellKey);


            var isCharged = PlayerCore.PlayerInput.actions["Charge"].IsPressed();
            if (isCharged)
                WizardAnimationController.PlayAnimation(WizardAnimationController.AnimationState.Charge);
        }

        private async UniTaskVoid TryDuplication(string spellKey)
        {
            var dups = PlayerBuff.BuffParameters.Where(value => value.BuffKey == BuffKey.Duplication).ToList();

            foreach (var buffParameter in dups)
            {
                await MyDelay(0.5f);
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