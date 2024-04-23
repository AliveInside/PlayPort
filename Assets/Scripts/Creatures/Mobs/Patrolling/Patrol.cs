using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Creatures.Mobs.Patrolling
{
    public abstract class Patrol : MonoBehaviour
    {
        public abstract IEnumerator DoPatrol();
    }
}
