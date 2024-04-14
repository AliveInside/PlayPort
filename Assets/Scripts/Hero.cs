using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
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
        private Animator _animator;
        private SpriteRenderer _sprite;
        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");

        private int _coins;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = new Vector2(_direction.x * _speed, _rigidbody.velocity.y);

            var isJumping = _direction.y > 0;
            var isGrounded = IsGrounded();

            if (isJumping)
            {
                if (isGrounded && _rigidbody.velocity.y <= 0)
                {
                    _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);            
                }
            } else if (_rigidbody.velocity.y > 0)
                {
                    _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
                } 
            
            _animator.SetBool(IsGroundKey, isGrounded);
            _animator.SetBool(IsRunningKey, _direction.x !=0);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                _sprite.flipX = false;
            }
            else if (_direction.x < 0)
            {
                _sprite.flipX = true;
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

        public void AddCoins(int coins)
        {
            if (coins != 0)
            {
                _coins += coins;
                Debug.Log($"{coins} coins added. Total coins: {_coins}");
            }
            else
            {
                _coins = 0;
            }
            
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
