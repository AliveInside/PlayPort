using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayPort.Components.ColliderBased
{
    public class EnterTriggerComponents : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (!collider.gameObject.IsInLayer(_layer)) return;

            if (!string.IsNullOrEmpty(_tag) && !collider.gameObject.CompareTag(_tag)) return;

            _action?.Invoke(collider.gameObject);

        }

        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {

        }
    }
}
