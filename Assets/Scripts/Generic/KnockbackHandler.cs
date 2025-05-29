using Interface;
using UnityEngine;

namespace Generic
{
    public class KnockbackHandler : MonoBehaviour
    {
        private Vector3 _knockbackVelocity;
        private float _knockbackTimer;

        private Rigidbody _rb;
        private CharacterController _controller;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            _controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (_knockbackTimer > 0)
            {
                if (_rb)
                {
                    _rb.AddForce(_knockbackVelocity, ForceMode.Impulse);
                    _knockbackTimer = 0; 
                }
                else if (_controller)
                {
                    _controller.Move(_knockbackVelocity * Time.deltaTime);
                    _knockbackTimer -= Time.deltaTime;
                }
            }
        }

        public void ApplyKnockback(Vector3 sourcePosition, float force, float duration)
        {
            Vector3 direction = (transform.position - sourcePosition).normalized;
            _knockbackVelocity = direction * force;
            _knockbackTimer = duration;
        }
    }
}
