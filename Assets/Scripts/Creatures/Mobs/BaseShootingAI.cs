using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayPort.Components;
using PlayPort.Components.ColliderBased;
using PlayPort.Utils;

namespace PlayPort.Creatures.Mobs
{
    public class BaseShootingAI : MonoBehaviour
    {
        [SerializeField] public LayerCheck _vision;

        [Header("Range")]
        [SerializeField] protected SpawnComponent _rangeAttack;
        [SerializeField] protected CoolDown _rangeCooldown;

        protected Animator _animator;
        private static readonly int RangeKey = Animator.StringToHash("range");

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                Debug.Log("I see you");
                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                    Debug.Log("I shoot you");
                }
            }
        }

        public void OnRangeAttack()
        {
            _rangeCooldown.Reset();
            _rangeAttack.Spawn();
        }

        protected void RangeAttack()
        {
            _animator.SetTrigger(RangeKey);
        }
    }
}
