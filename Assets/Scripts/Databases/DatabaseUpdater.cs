using UnityEditor;
using UnityEngine;

namespace Databases
{
#if UNITY_EDITOR

    public class DatabaseUpdater : EditorWindow
    {
        [MenuItem("Assets/UpdateAllDatabase")]
        private static void UpdateAllDatabase()
        {
            AttackDatabase.LoadOnEditor().Update();
            Debug.Log("UpdateAttack");

            CharacterDatabase.LoadOnEditor().Update();
            Debug.Log("UpdateCharacter");

            SpellDatabase.LoadOnEditor().Update();
            Debug.Log("UpdateSpell");
        }
    }
#endif
}