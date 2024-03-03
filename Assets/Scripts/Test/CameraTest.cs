using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class CameraTest : MonoBehaviour
    {
        [SerializeField] private Camera cam;


        // Update is called once per frame
        void Update()
        {
            var mousePos = Mouse.current.position.value;

            var v = new Vector3(mousePos.x, mousePos.y, -cam.transform.position.z);

            var point = cam.ScreenToWorldPoint(v);

            transform.position = point;
        }
    }
}