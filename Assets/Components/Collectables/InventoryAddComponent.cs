using System.Collections;
using System.Collections.Generic;
using PlayPort.Creatures;
using PlayPort.Model.Definitions;
using UnityEngine;

namespace PlayPort.Components.Collectables
{
    public class InventoryAddComponent : MonoBehaviour
    {
        [InventoryId] [SerializeField] private string _id;
        [SerializeField] private int _value;

        public void Add(GameObject gameObject)
        {
            var hero = gameObject.GetComponent<Hero>();
            hero?.AddInInventory(_id, _value);
        }
    }
}