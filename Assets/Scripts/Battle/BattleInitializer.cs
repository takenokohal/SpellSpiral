using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.Character;
using Battle.Character.Servant;
using Cysharp.Threading.Tasks;
using Databases;
using Others.Input;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

namespace Battle
{
    public class BattleInitializer : MonoBehaviour
    {
        [Inject] private readonly BattleLoop _battleLoop;
        [Inject] private readonly MyInputManager _myInputManager;
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly CharacterFactory _characterFactory;

        public static string StaticStageName { get; set; }

        [SerializeField, ValueDropdown(nameof(BossKeys))]
        private string serializedStageName;


        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            _myInputManager.PlayerInput.SwitchCurrentActionMap("Player");

            var bossName = StaticStageName ?? serializedStageName;

            var bossData = _characterDatabase.Find(bossName);

            var bossInstance = _characterFactory.CreateAndInject(
                bossData.CharacterBase,
                null, Vector2.zero);

            var stageObjectInstance = Instantiate(bossData.StageObject);

            await bossInstance.WaitUntilInitialize();

            _battleLoop.SendEvent(BattleEvent.SceneStart);
        }


        private static IEnumerable<string> BossKeys()
        {
#if UNITY_EDITOR
            var db = CharacterDatabase.LoadOnEditor();
            return db.CharacterDictionary.Where(value => value.Value.CharacterType == CharacterType.Boss)
                .Select(value => value.Key);

#else
            return null;
#endif
        }
    }
}