using System;
using System.Collections;
using System.Collections.Generic;
using PlayPort.Components;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

namespace PlayPort.Creatures.Mobs.Patrolling
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] protected LayerCheck _vision;
        [SerializeField] protected LayerCheck _canAttack;
        [SerializeField] protected SpawnListComponent _particles;
        [SerializeField] private float _noticeDelay = 0.5f;
        [SerializeField] protected float _lostDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 1f;

        private IEnumerator _current;
        private GameObject _target;
        protected Creature _creature;
        private Animator _animator;
        protected Patrol _patrol;
        private Collider2D _collider;
        private bool _isDead;
        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");

        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
            _collider = GetComponent<Collider2D>();
        }

        protected virtual void Start()
        {
            StartState(_patrol.DoPatrol());
        }

        public void OnHeroInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;
            StartState(NoticeHero());
        }

        private IEnumerator NoticeHero()
        {
            _particles.Spawn("Notice");
            yield return new WaitForSeconds(_noticeDelay);
            StartState(PursuitOfHero());
        }

        private void LookAtHero()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        protected virtual IEnumerator PursuitOfHero()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }
                
                yield return null;
            }

            _creature.SetDirection(Vector2.zero);
            _particles.Spawn("Lost");
            yield return new WaitForSeconds(_lostDelay);
            StartState(_patrol.DoPatrol());
        }

        protected IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }

            StartState(PursuitOfHero());
        }

        protected void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetDirection(direction);
        }

        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }

        // private IEnumerator Patrolling()
        // {
        //     yield return null;
        // }

        protected void StartState(IEnumerator coroutine)
        {
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
            {
                StopCoroutine(_current);
            }
    
            // if (!_isDead)
            // {
                _current = coroutine;
                StartCoroutine(coroutine);
                //_current = StartCoroutine(coroutine);
            // }
        }

        public void OnDie()
        {
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);
            _creature.SetDirection(Vector2.zero);

            if (_current != null)
            {
                StopCoroutine(_current);
            }
        }
    }
}
