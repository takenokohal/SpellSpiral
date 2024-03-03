using UnityEngine;

namespace Test
{
    public class DeleteLOD : MonoBehaviour
    {
        private void Start()
        {
            var lods = FindObjectsByType<LODGroup>(FindObjectsSortMode.None);
            Debug.Log(lods.Length);
            
            foreach (var lodGroup in lods)
            {
                lodGroup.gameObject.SetActive(false);
            }


            var lights = FindObjectsByType<Light>(FindObjectsSortMode.None);

            Debug.Log(lights.Length);

            var particles = FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);

            Debug.Log(particles.Length);

            foreach (var particle in particles)
            {
                particle.gameObject.SetActive(false);
            }
        }
    }
}