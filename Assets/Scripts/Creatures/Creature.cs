using System.Collections;
using System.Collections.Generic;
using PlayPort.Components;
using UnityEngine;
using PlayPort.Components.ColliderBased;

namespace PlayPort.Creatures
{
    public class Creature : MonoBehaviour
    {
        [Space] [Header("Generic stats")]
        [SerializeField] private bool _invertScale;
        [SerializeField] protected float _speed;
        [SerializeField] protected float _jumpForce;
        [SerializeField] private float _damageJumpForce;

        [Space] [Header("Checks")]
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected LayerCheck _groundCheck;

        [Space] [Header("Attack")]
        [SerializeField] private int _damage;
        [SerializeField] private CheckCircleOverlap _attackRange;

        [Space] [Header("Particles")]
        [SerializeField] protected SpawnListComponent _particles;

        protected Vector2 Direction;
        protected Rigidbody2D Rigidbody;
        protected Animator Animator;
        protected bool IsGrounded;
        private bool _isJumping;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Regeneration = Animator.StringToHash("regeneration");
        private static readonly int AttackKey = Animator.StringToHash("attack");
        
        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }

        public void SetDirection(Vector2 direction)
        {
            Direction = direction;
        }

        protected virtual void Update()
        {
            IsGrounded = _groundCheck.IsTouchingLayer; // stream
        }

        // protected virtual bool IsGrounded()
        // {
        //     // Check #1 (with ray)
        //     // var hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _groundLayer);
        //     // return hit.collider != null;

        //     // Check #2 (with circle)
        //     var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector2.down, 0, _groundLayer);
        //     return hit.collider != null;
        // }

        
        private void FixedUpdate()
        {
            var xVelocity = Direction.x * _speed;
            var yVelocity = CalculcateYVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            Animator.SetBool(IsGroundKey, IsGrounded);
            Animator.SetBool(IsRunningKey, Direction.x !=0);
            Animator.SetFloat(VerticalVelocityKey, Rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private void UpdateSpriteDirection()
        {
            var multiplier = _invertScale ? -1 : 1;
            if (Direction.x > 0)
            {
                transform.localScale = new Vector3(1 * multiplier, 1, 1);
            }
            else if (Direction.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }
        

        protected virtual float CalculcateYVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded) 
            {
                _isJumping = false;
            }
            
            if (isJumpPressing)
            {
                _isJumping = true;

                var isFalling = Rigidbody.velocity.y <= 0.001f;

                yVelocity = isFalling ? CalculcateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            } 

            return yVelocity;
        }

        protected virtual float CalculcateJumpVelocity(float yVelocity)
        {
            if (IsGrounded)
            {
                yVelocity = _jumpForce; 
                _particles.Spawn("Jump");   
            }

            return yVelocity;
        }

        public virtual void TakeDamage()
        {
            _isJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpForce);
        }

        public virtual void TakeRegeneration()
        {
            Animator.SetTrigger(Regeneration);
        }

        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
            _particles.Spawn("Attack");
            OnDoAttack();
        }

        public void OnDoAttack()
        {
            _attackRange.Check();
        } 

    }
}
