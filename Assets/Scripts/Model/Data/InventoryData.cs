using System;
using System.Collections;
using System.Collections.Generic;
using PlayPort.Model.Definitions;
using UnityEngine;

namespace PlayPort.Model.Data
{
    [Serializable]
    public class InventoryData
    {
        [SerializeField] private List<InventoryItemData> _inventory = new List<InventoryItemData>();

        public delegate void OnInventoryChanged(string id, int value);

        // Same
        // public Action<string, int> OnChanged;

        public OnInventoryChanged OnChanged;

        public void Add(string id, int value)
        {
            if (value <= 0) return;

            var itemDef = DefsFacade.I.Items.Get(id);
            if (itemDef.IsVoid) return;

            //Stack check
            if (itemDef.IsStack)
            {
                AddStackableItems(id, value);
            }
            else
            {
                AddNonStackableItems(id, value);
            }

            OnChanged?.Invoke(id, Count(id));
        }

        private void AddStackableItems(string id, int value)
        {
            var isFull = _inventory.Count > DefsFacade.I.Player.InventorySize;
            var item = GetItem(id);
            if (item == null)
            {
                //Full inventory catching
                if (isFull) return;

                item = new InventoryItemData(id);
                _inventory.Add(item);
            }

            item.Value += value;
        }

        private void AddNonStackableItems(string id, int value)
        {
            //Full inventory catching ver. 2.0
            var itemLasts = DefsFacade.I.Player.InventorySize - _inventory.Count;
            value = Mathf.Min(itemLasts, value);
            for (int i = 0; i < value; i++)
            {
                //Full inventory catching
                // var isFull = _inventory.Count > DefsFacade.I.Player.InventorySize;
                // if (isFull) return;
                var item = new InventoryItemData(id) {Value = 1};
                _inventory.Add(item);
            }
        }

        public void Remove(string id, int value)
        {
            var itemDef = DefsFacade.I.Items.Get(id);
            Debug.Log("Deleting item " + itemDef);
            Debug.Log("Item IsVoid " + itemDef.IsVoid);
            Debug.Log("Item IsStack " + itemDef.IsStack);
            if (itemDef.IsVoid) return;

            //Stack check
            if (itemDef.IsStack)
            {
                RemoveStackableItems(id, value);
            }
            else
            {
                RemoveNonStackableItems(id, value);
            }

            OnChanged?.Invoke(id, Count(id));
        }

        private void RemoveStackableItems(string id, int value)
        {
            var item = GetItem(id);
            if (item == null) return;

            item.Value -= value;

            if (item.Value <= 0)
                _inventory.Remove(item);
        }

        private void RemoveNonStackableItems(string id, int value)
        {
            for (int i = 0; i < value; i++)
            {
                var item = GetItem(id);
                if (item == null) return;

                _inventory.Remove(item);
            }
        }


        private InventoryItemData GetItem(string id)
        {
            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    return itemData;
            }
            return null;
        }

        public int Count(string id)
        {
            var count = 0;

            foreach (var itemData in _inventory)
            {
                if (itemData.Id == id)
                    count += itemData.Value;
            }

            return count;
        }
    }

    [Serializable]
    public class InventoryItemData
    {
        [InventoryId] public string Id;
        public int Value;

        public InventoryItemData(string id)
        {
            Id = id;
        }
    }
}
