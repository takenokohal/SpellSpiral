using TMPro;
using UnityEngine;

namespace HomeScene
{
    public class HomeMenuItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text tmpText;

        public string Text
        {
            get => tmpText.text;
            set => tmpText.text = value;
        }

        public void SetTextEnable(bool isOn) => tmpText.enabled = isOn;
        
    }
}