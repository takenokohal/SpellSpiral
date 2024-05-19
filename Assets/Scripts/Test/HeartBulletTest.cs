using UnityEngine;

namespace Test
{
    public class HeartBulletTest : MonoBehaviour
    {
        [SerializeField] private Rigidbody[] bullet;
        [SerializeField] private float firstAngle;
        [SerializeField] private float firstSpeed;

        [SerializeField] private float accelAngle;
        [SerializeField] private float accel;


        private void Start()
        {
            for (int i = 0; i < bullet.Length; i++)
            {
                var angle = firstAngle * Mathf.Deg2Rad;
                if (i > 0)
                    angle *= -1;
                bullet[i].velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * firstSpeed;
            }
        }

        private void FixedUpdate()
        {
            for (var i = 0; i < bullet.Length; i++)
            {
                var angle = accelAngle * Mathf.Deg2Rad;
                if (i > 0)
                    angle *= -1;
                bullet[i].AddForce(new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * accel);
            }
        }
    }
}