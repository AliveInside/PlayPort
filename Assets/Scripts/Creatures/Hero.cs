using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using PlayPort.Components;
using PlayPort.Model;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using PlayPort.Components.ColliderBased;

namespace PlayPort.Creatures
{
    public class Hero : Creature
    {
        [Space] [Header("Generic stats")]
        [SerializeField] private float _slamDownVelocity;

        [Space] [Header("Checks")]
 
        [SerializeField] private float _interactRadius;
        [SerializeField] private LayerCheck _wallCheck;

        [Space] [Header("Attack")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Space] [Header("Particles")]

        [SerializeField] private ParticleSystem _hitParticleSystem;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        private SpriteRenderer _sprite;

        private bool _allowDoubleJump;
        private bool _isOnWall;
        private float _defaultGravityScale;

        private GameSession _session;


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale; 
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            var health = GetComponent<HealthComponent>();

            health.SetHealth(_session.Data.Health);
            UpdateHeroWeapon();
        }

        protected override void Update()
        {   
            base.Update();
            
            if (_wallCheck.IsTouchingLayer && Direction.x == transform.localScale.x)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
        }


        protected override float CalculcateYVelocity()
        {
            var isJumpPressing = Direction.y > 0;

            if (IsGrounded || _isOnWall) 
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }
            
            return base.CalculcateYVelocity();
        }

        protected override float CalculcateJumpVelocity(float yVelocity)
        {
            if (!IsGrounded && _allowDoubleJump)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpForce;
            }

            return base.CalculcateJumpVelocity(yVelocity);
        }

        public void AddCoins(int coins)
        {
            if (coins != 0)
            {
                _session.Data.Coins += coins;
                Debug.Log($"{coins} coins added. Total coins: {_session.Data.Coins}");
            }
            else
            {
                _session.Data.Coins = 0;
            }
        }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Health = currentHealth;
        }

        public void SpawnCoins()
        {
            var numCoinsToSpawn = Mathf.Min(_session.Data.Coins, 5);
            _session.Data.Coins -= numCoinsToSpawn;

            var burst = _hitParticleSystem.emission.GetBurst(0);
            burst.count = numCoinsToSpawn;
            _hitParticleSystem.emission.SetBurst(0, burst);

            _hitParticleSystem.gameObject.SetActive(true);
            _hitParticleSystem.Play();
        }

        public void JumpBuff(float buffCoefficient)
        {
            _jumpForce *= buffCoefficient;
        }

        public void SpeedBuff(float buffCoefficient)
        {
            _speed *= buffCoefficient;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_groundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity)
                {
                    _particles.Spawn("SlamDown");
                }
            }
        }

        public override void Attack()
        {
            if (!_session.Data.isArmed) return;

            base.Attack();
        }

        public void ArmHero()
        {
            _session.Data.isArmed = true;
            UpdateHeroWeapon();
        }
        
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.isArmed ? _armed : _unarmed;
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }




// #if UNITY_EDITOR
//         private void OnDrawGizmos()
//         {
//             // Check #1 (with ray)
//             //Debug.DrawRay(transform.position, Vector3.down, IsGrounded() ? Color.green : Color.red);

//             // Gizmos.color = IsGrounded() ? Color.green : Color.red;
//             // Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);

//             Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
//             Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
//         }
// #endif

    }
}
