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
using PlayPort.Utils;

namespace PlayPort.Creatures
{
    public class Hero : Creature
    {
        [Space] [Header("Generic stats")]
        [SerializeField] private float _slamDownVelocity;

        [Space] [Header("Checks")]
        [SerializeField] private LayerCheck _wallCheck;

        [Space] [Header("Attack")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _unarmed;
        [SerializeField] private CoolDown _throwCoolDown;
        
        [SerializeField] private int _swordsAmmo;

        [Space] [Header("Particles")]

        [SerializeField] private ParticleSystem _hitParticleSystem;
        [SerializeField] private CheckCircleOverlap _interactionCheck;

        private SpriteRenderer _sprite;

        private bool _allowDoubleJump;
        private bool _isOnWall;
        private float _defaultGravityScale;
        private HealthComponent _health;

        private GameSession _session;

        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int ClimbingKey = Animator.StringToHash("is-climbing");
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale; 
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _health = GetComponent<HealthComponent>();

            _swordsAmmo = SwordCount;
            Debug.Log("Hero at start have a " + _swordsAmmo);

            _health.SetHealth(_session.Data.Health);
            UpdateHeroWeapon();
        }



        protected override void Update()
        {   
            base.Update();

            var moveToSameDirection = Direction.x * transform.lossyScale.x > 0;
            
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }

            Animator.SetBool(ClimbingKey, _isOnWall);
        }

        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
        }

        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdateHeroWeapon();
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
            if (!IsGrounded && _allowDoubleJump && !_isOnWall)
            {
                _particles.Spawn("Jump");
                _allowDoubleJump = false;
                return _jumpForce;
            }

            return base.CalculcateJumpVelocity(yVelocity);
        }

        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }

        // public void AddCoins(int coins)
        // {
        //     if (coins != 0)
        //     {
        //         _session.Data.Coins += coins;
        //         Debug.Log($"{coins} coins added. Total coins: {_session.Data.Coins}");
        //     }
        //     else
        //     {
        //         _session.Data.Coins = 0;
        //     }
        // }

        public override void TakeDamage()
        {
            base.TakeDamage();

            if (CoinCount > 0)
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
            var numCoinsToSpawn = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToSpawn);

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
            if (SwordCount <= 0) return;

            base.Attack();
        }
        
        private void UpdateHeroWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _unarmed;
        }

        public void Interact()
        {
            _interactionCheck.Check();
        }

        public void OnDoThrow()
        {
            _particles.Spawn("Throw");
            _session.Data.Inventory.Remove("Sword", 1);
        }

        public void Throw()
        {
            if (_throwCoolDown.IsReady && SwordCount > 1)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCoolDown.Reset();
            }
        }

        public void SwordsCountChange(int amount)
        {
            _session.Data.Inventory.Add("Sword", amount);
        }

        public void UsePotion()
        {
            if (_session.Data.Inventory.Count("HealthPotion") > 0)
            {
                _health.HealthChange(5);
                _session.Data.Inventory.Remove("HealthPotion", 1);
            }
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
