using System.Collections;
using System.Collections.Generic;
using PlayPort.Model.Definitions;
using PlayPort.Model;
using UnityEngine;
using UnityEngine.Events;
using System.Threading;
using PlayPort.Model.Data;


namespace PlayPort.Components.Interactions
{
    public class RequireItemComponent : MonoBehaviour
    {
        [SerializeField] private InventoryItemData[] _requirements;
        [SerializeField] private bool _removeAfterUse;

        [SerializeField] private UnityEvent _onSuccess;
        [SerializeField] private UnityEvent _onFailure;

        public void Check()
        {
            var session = FindObjectOfType<GameSession>();
            var allRequirementsCompleted = true;

            foreach (var inventoryItem in _requirements)
            {
                var numItems = session.Data.Inventory.Count(inventoryItem.Id);
                if (numItems < inventoryItem.Value)
                    allRequirementsCompleted = false;
            }

            if (allRequirementsCompleted)
            {
                if (_removeAfterUse)
                {
                    foreach (var inventoryItem in _requirements)
                        session.Data.Inventory.Remove(inventoryItem.Id, inventoryItem.Value);
                }
                _onSuccess?.Invoke();
            }
            else
            {
                _onFailure?.Invoke();
            }
        }
    }
}
