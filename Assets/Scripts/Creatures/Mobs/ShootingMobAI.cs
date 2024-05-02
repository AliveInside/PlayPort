using System;
using System.Collections;
using System.Collections.Generic;
using PlayPort.Components;
using PlayPort.Components.ColliderBased;
using PlayPort.Utils;
using TMPro;
using UnityEngine;

namespace PlayPort.Creatures.Mobs
{
    public class ShootingMobAI : BaseShootingAI
    {
        [Header("Melee")]
        [SerializeField] private LayerCheck _meleeCanAttack;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private CoolDown _meleeCooldown;
        private static readonly int MeleeKey = Animator.StringToHash("melee");

        protected override void Update()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                        MeleeAttack();
                    return;
                }

                base.Update();
            }
        }

        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
        }

        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(MeleeKey);
        }
    }
}
