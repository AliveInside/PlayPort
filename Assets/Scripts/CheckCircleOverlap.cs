using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace PlayPort
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1f;
        private Collider2D[] _interactResult = new Collider2D[5];

        public GameObject[] GetObjectsInRange()
        {
            var size = Physics2D.OverlapCircleNonAlloc(transform.position, _radius, _interactResult);
            var overlaps = new List<GameObject>();

            for (var i = 0; i < size; i++)
            {
                overlaps.Add(_interactResult[i].gameObject);
            }

            return overlaps.ToArray();
        }

        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TransparentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }
    }
}
