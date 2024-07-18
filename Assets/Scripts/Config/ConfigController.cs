using DeckEdit.SaveData;

namespace Config
{
    public class ConfigController
    {
        private const string ConfigKey = "Config";

        public ConfigData CurrentConfigData { get; set; }

        public bool DataIsLoaded => CurrentConfigData != null;

        private void Load()
        {
            var data = EasySaveWrapper.Load<ConfigData>(ConfigKey);
            if (data != null)
            {
                CurrentConfigData = data;
                return;
            }

            CurrentConfigData = ConfigData.GetDefaultData();
        }

        public void Save()
        {
            EasySaveWrapper.Save(ConfigKey, CurrentConfigData);
        }

        public ConfigController()
        {
            Load();
        }
    }
}