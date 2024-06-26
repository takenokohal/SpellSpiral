using Battle.Character;
using Battle.Character.Enemy;
using Battle.Character.Player;
using Battle.Character.Player.Buff;
using Battle.Character.Servant;
using Battle.CommonObject.MagicCircle;
using Databases;
using VContainer;

namespace Battle.PlayerSpell
{
    public class SpellFactory
    {
        [Inject] private readonly SpellDatabase _spellDatabase;
        [Inject] private readonly AllCharacterManager _allCharacterManager;
        [Inject] private readonly PlayerCore _playerCore;
        [Inject] private readonly MagicCircleFactory _magicCircleFactory;
        [Inject] private readonly CharacterFactory _characterFactory;
        [Inject] private readonly AttackDatabase _attackDatabase;


        public void Create(string spellKey)
        {
            var spellData = _spellDatabase.Find(spellKey);
            var instance = spellData.SpellBase.Construct(
                _playerCore,
                _allCharacterManager,
                _magicCircleFactory,
                _spellDatabase,
                _attackDatabase,
                spellData, _characterFactory);
        }
    }
}