using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

namespace PlayPort
{
    public class Hero : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);
            var isJumping = _direction.y > 0;

            if (isJumping)
            {
                if (IsGrounded())
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);            
                }
            } else if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
                } 
        }
        
        public void SetDirection(Vector2 direction)
        {
            _direction = direction;
        }

        public void Speech()
        {
            Debug.Log("Speeeeech");
        }

        private bool IsGrounded()
        {
            // Check #1 (with ray)
            // var hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _groundLayer);
            // return hit.collider != null;

            // Check #2 (with circle)
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
            return hit.collider != null;
        }

        private void OnDrawGizmos()
        {
            // Check #1 (with ray)
            //Debug.DrawRay(transform.position, Vector3.down, IsGrounded() ? Color.green : Color.red);

            Gizmos.color = IsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);
        }
    }
}
