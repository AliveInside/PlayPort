using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Model.Definitions
{
    [CreateAssetMenu(menuName = "Defs/InventoryItems", fileName = "InventoryItems")]
    public class InventoryItemDef : ScriptableObject
    {
        [SerializeField] private ItemDef[] _items;

        public ItemDef Get(string id)
        {
            foreach (var item in _items)
            {
                if (item.Id == id)
                    return item;
            }

            return default;
        }

#if UNITY_EDITOR
    public ItemDef[] ItemsForEditor => _items;
#endif
    }

    [Serializable]
    public struct ItemDef
    {
        [SerializeField] private string _id;
        [SerializeField] private bool _isStack;
        public string Id => _id;
        public bool IsStack => _isStack;

        public bool IsVoid => string.IsNullOrEmpty(_id);
    }
}
