using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayPort.Components
{
    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject target)
        {
            var interactable = target.GetComponent<InteractableComponent>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
