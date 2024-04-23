using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.Events;
using System.Linq;


namespace PlayPort.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _OnOverlap;
        private Collider2D[] _interactResult = new Collider2D[10];

        // public GameObject[] GetObjectsInRange()
        // {
        //     var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactResult);
        //     var overlaps = new List<GameObject>();

        //     for (var i = 0; i < size; i++)
        //     {
        //         overlaps.Add(_interactResult[i].gameObject);
        //     }

        //     return overlaps.ToArray();
        // }

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }

        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position,
            _radius,
            _interactResult,
            _mask);
            var overlaps = new List<GameObject>();

            for (var i = 0; i < size; i++)
            {
                var overlapResult = _interactResult[i];
                var isInTags = _tags.Any(tag => _interactResult[i].CompareTag(tag));
                if (isInTags)
                    _OnOverlap.Invoke(_interactResult[i].gameObject);
            }

        }

        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {  
        }
    }
}
