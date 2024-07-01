using UnityEngine;

namespace Others.Utils
{
    public static class PathsAndURL
    {
        private const string DatabaseFolderPath = "Assets/Databases/";
        private const string SpellIconFolderPath = "Assets/Pictures/SpellIcons/";
        private const string SpellBaseFolderPath = "Assets/Prefabs/Spells/";
        private const string VideoFolderPath = "Assets/Videos/SpellVideos";
        private const string CharacterBaseFolderPath = "Assets/Prefabs/Characters/";
        private const string StageObjectPath = "Assets/Prefabs/StageObjects/";

        public static string CreateDatabasePath<T>() where T : ScriptableObject =>
            DatabaseFolderPath + typeof(T).Name + ".asset";

        public static string CreateSpellIconPath(string spellKey) => SpellIconFolderPath + spellKey + ".png";
        public static string CreateSpellBasePath(string spellKey) => SpellBaseFolderPath + spellKey + ".prefab";

        public static string CreateVideoPath(string spellKey) => VideoFolderPath + spellKey + ".mp4";


        public static string CreateCharacterBasePath(string characterKey) =>
            CharacterBaseFolderPath + characterKey + "/" + characterKey + ".prefab";

        public static string CreateStageObjectPath(string characterKey) => StageObjectPath + characterKey + ".prefab";

        private const string SpreadSheetURL =
            "https://docs.google.com/spreadsheets/d/1HtYMkw6D6IDn_tYoRDSFNL5LyQpuLhGzJPkTJdNNa9U/gviz/tq?tqx=out:csv&sheet=";

        public static string AttackDatabaseSpreadSheetURL => SpreadSheetURL + "AttackData";

        public static string MessageDatabaseSpreadSheetURL => SpreadSheetURL + "MessageData";

        public static string SpellDatabaseSpreadSheetURL => SpreadSheetURL + "NormalSpell";

        public static string CharacterDatabaseSpreadSheetURL => SpreadSheetURL + "CharacterData";
    }
}