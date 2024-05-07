using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HomeScene
{
    public class IconAndTitle : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text titleText;

        public Image Image => image;
        public TMP_Text TitleText => titleText;

        public IconAndTitle CreateFromPrefab(Parameter parameter, Transform parent)
        {
            var instance = Instantiate(this, parent);
            instance.Image.sprite = parameter.sprite;
            instance.TitleText.text = parameter.title;

            return instance;
        }

        [Serializable]
        public class Parameter
        {
            public Sprite sprite;
            public string title;
        }
    }
}