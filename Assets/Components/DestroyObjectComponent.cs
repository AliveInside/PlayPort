using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;
        
        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
        }
    }
}
