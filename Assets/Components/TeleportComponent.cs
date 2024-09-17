﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components
{
    public class TeleportComponent : MonoBehaviour
    {
        [SerializeField] private Transform _destination;
        public void Teleport(GameObject target)
        {
            target.transform.position = _destination.position;
        }
    }
}
