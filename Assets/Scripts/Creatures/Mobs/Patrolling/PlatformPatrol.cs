using System.Collections;
using PlayPort.Components.ColliderBased;
using UnityEngine;


namespace PlayPort.Creatures.Mobs.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LineCastCheck _groundCheck;
        [SerializeField] private LineCastCheck _obstacleCheck;
        [SerializeField] private int _direction;
        [SerializeField] private Creature _creature;
        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (_groundCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
                {
                    _creature.SetDirection(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _creature.SetDirection(new Vector2(_direction, 0));
                }

                yield return null;
            }
        }

    }
}
