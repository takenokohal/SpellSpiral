using UnityEngine;

namespace DeckEdit.SaveData
{
    public static class EasySaveWrapper
    {
        public static void Save<T>(string key, T data)
        {
            var json = JsonUtility.ToJson(data);
            ES3.Save(key, json);
        }

        public static T Load<T>(string key) where T : class
        {
            if (!ES3.KeyExists(key))
                return null;


            var json = ES3.Load<string>(key);

            return JsonUtility.FromJson<T>(json);
        }
    }
}