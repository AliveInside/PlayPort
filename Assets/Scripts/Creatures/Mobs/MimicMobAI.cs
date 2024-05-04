using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayPort.Components;

namespace PlayPort.Creatures.Mobs.Patrolling
{
    public class MimicMobAI : MobAI
    {
        private SpriteRenderer _parentSpriteRenderer;

        protected override void Start()
        {
            base.Start();
            _parentSpriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        }

        protected override IEnumerator PursuitOfHero()
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

            transform.parent.position = transform.position;
            gameObject.SetActive(false);
            _parentSpriteRenderer.enabled = true;
        }
    }
}
