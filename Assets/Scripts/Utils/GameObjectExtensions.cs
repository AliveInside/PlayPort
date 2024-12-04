using System;
using UnityEngine;

namespace PlayPort
{
    public static class GameObjectExtensions
    {
        public static bool IsInLayer(this GameObject gameObject, LayerMask layerMask)
        {
            //0001 gameObject.layer
            //0110 layerMask
            //0111 result
            //0111 != 0110 
            return layerMask == (layerMask | 1 << gameObject.layer);
        }
    }
}
