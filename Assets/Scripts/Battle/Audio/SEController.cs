/*using UnityEngine;

namespace Battle.Audio
{
    public class SEController : MonoBehaviour
    {
        private static SEController _instance;

        public static SEController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SEController>();
                }

                return _instance;
            }
        }

        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip magicCircleClip;
        [SerializeField] private AudioClip shotClip;

        private void Start()
        {
            
        }

        public void PlayMagicCircle()
        {
            audioSource.PlayOneShot(magicCircleClip);
        }

        public void PlayShot()
        {
            audioSource.PlayOneShot(shotClip);
        }
    }
}*/