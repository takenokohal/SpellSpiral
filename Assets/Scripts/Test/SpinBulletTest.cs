using UnityEngine;

namespace Test
{
    public class SpinBulletTest : MonoBehaviour
    {
        [SerializeField] private float firstSpeed;
        [SerializeField] private float power;


        private Rigidbody _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.velocity = Vector2.up * firstSpeed;
        }

        private void FixedUpdate()
        {
            var velocity = _rigidbody.velocity.normalized;
            var forceDirection = Vector3.Cross(velocity, Vector3.forward);

            _rigidbody.AddForce(forceDirection * power);
        }
    }
}