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

namespace Battle.System.Main
{
    public class BattleInitializer : MonoBehaviour
    {
        [Inject] private readonly BattleLoop _battleLoop;
        [Inject] private readonly MyInputManager _myInputManager;
        [Inject] private readonly CharacterDatabase _characterDatabase;
        [Inject] private readonly CharacterFactory _characterFactory;

        public static string StaticStageKey { get; set; }

        [SerializeField, ValueDropdown(nameof(BossKeys))]
        private string serializedStageKey;


        private void Start()
        {
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            _myInputManager.PlayerInput.SwitchCurrentActionMap("Player");

            var bossKey = StaticStageKey ?? serializedStageKey;

            var bossData = _characterDatabase.Find(bossKey);

            var bossInstance = _characterFactory.CreateAndInject(
                bossKey,
                null, Vector2.zero);

            if (bossData.StageObject != null)
                Instantiate(bossData.StageObject);

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