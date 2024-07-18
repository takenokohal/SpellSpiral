using System;
using UnityEngine;

namespace Config
{
    [Serializable]
    public class ConfigData
    {
        public SystemLanguage language;

        private ConfigData(SystemLanguage language)
        {
            this.language = language;
        }

        public static ConfigData GetDefaultData()
        {
            return new ConfigData(Application.systemLanguage);
        }
    }
}