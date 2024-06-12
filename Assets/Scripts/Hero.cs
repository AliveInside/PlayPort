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

namespace PlayPort
{
    public class Hero : MonoBehaviour
    {
        [Space] [Header("Generic stats")]
        [SerializeField] private float _speed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _damageJumpForce;
        [SerializeField] private float _slamDownVelocity;

        [Space] [Header("Checks")]
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;
        [SerializeField] private float _interactRadius;
        [SerializeField] private LayerMask _interactLayer;

        [Space] [Header("Attack")]
        [SerializeField] private int _damage;
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;

        [Space] [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepsParticle;
        [SerializeField] private SpawnComponent _jumpParticle;
        [SerializeField] private SpawnComponent _slamDownParticle;
        [SerializeField] private SpawnComponent _attackParticle;
        [SerializeField] private ParticleSystem _hitParticleSystem;

        private Collider2D[] _interactResult = new Collider2D[1];
        private Vector2 _direction;
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private SpriteRenderer _sprite;

        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;

        private static readonly int IsGroundKey = Animator.StringToHash("is-ground");
        private static readonly int IsRunningKey = Animator.StringToHash("is-running");
        private static readonly int VerticalVelocityKey = Animator.StringToHash("vertical-velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Regeneration = Animator.StringToHash("regeneration");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        private GameSession _session;



        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            var health = GetComponent<HealthComponent>();
            health.SetHealth(_session.Data.Health);

            UpdateHeroWeapon();
        }

        private void Update()
        {
            _isGrounded = IsGrounded();
        }

        private void FixedUpdate()
        {
            var xVelocity = _direction.x * _speed;
            var yVelocity = CalculcateYVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);

            _animator.SetBool(IsGroundKey, _isGrounded);
            _animator.SetBool(IsRunningKey, _direction.x !=0);
            _animator.SetFloat(VerticalVelocityKey, _rigidbody.velocity.y);

            UpdateSpriteDirection();
        }

        private float CalculcateYVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _direction.y > 0;

            if(_isGrounded) 
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }
            
            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculcateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            } 

            return yVelocity;
        }

        private float CalculcateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;

            if(!isFalling) return yVelocity;
            if (_isGrounded)
            {
                _jumpParticle.Spawn();
                yVelocity = _jumpForce;        
            }
            else if (_allowDoubleJump)
            {
                yVelocity = _jumpForce;
                _jumpParticle.Spawn();
                _allowDoubleJump = false;
            }

            return yVelocity;
        }

        private void UpdateSpriteDirection()
        {
            if (_direction.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_direction.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
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
                _session.Data.Coins += coins;
                Debug.Log($"{coins} coins added. Total coins: {_session.Data.Coins}");
            }
            else
            {
                _session.Data.Coins = 0;
            }
        }

        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);

            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }

        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Health = currentHealth;
        }

        public void TakeRegeneration()
        {
            _animator.SetTrigger(Regeneration);
        }

        public void SpawnFootStepsParticle()
        {
            _footStepsParticle.Spawn();
        }

        public void SpawnJumpParticle()
        {
            _jumpParticle.Spawn();
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
                    _slamDownParticle.Spawn();
                }
            }
        }

        public void Attack()
        {
            if (!_session.Data.isArmed) return;

            _animator.SetTrigger(AttackKey);
            _attackParticle.Spawn();
        }

        public void OnDoAttack()
        {
            var gameobjects = _attackRange.GetObjectsInRange();

            foreach (var gameobject in gameobjects)
            {
                // Debug.Log(gameobjects.Length);
                var hp = gameobject.GetComponent<HealthComponent>();
                if (hp != null && gameobject.CompareTag("Enemy"))
                {
                    // Debug.Log(gameobject);
                    hp.HealthChange(-_damage);
                }      
            } 
        } 

        public void ArmHero()
        {
            _session.Data.isArmed = true;
            UpdateHeroWeapon();
        }
        
        private void UpdateHeroWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.isArmed ? _armed : _unarmed;
        }

        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _interactRadius, _interactResult, _interactLayer);
            for (int i = 0; i < size; i++)
            {
                var interactable = _interactResult[i].GetComponent<InteractableComponent>();
                interactable?.Interact();
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

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            // Check #1 (with ray)
            //Debug.DrawRay(transform.position, Vector3.down, IsGrounded() ? Color.green : Color.red);

            // Gizmos.color = IsGrounded() ? Color.green : Color.red;
            // Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);

            Handles.color = IsGrounded() ? HandlesUtils.TransparentGreen : HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position + _groundCheckPositionDelta, Vector3.forward, _groundCheckRadius);
        }
#endif

    }
}
