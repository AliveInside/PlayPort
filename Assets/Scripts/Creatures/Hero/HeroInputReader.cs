using System.Collections;
using System.Collections.Generic;
using PlayPort.Components;
using PlayPort.Creatures;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayPort
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;
        

        public void OnSpeech(InputAction.CallbackContext callbackContext)
        {
            // if (callbackContext.canceled)
            // {
            //     _hero.Speech();
            // }
        }

        public void OnMovement(InputAction.CallbackContext callbackContext)
        {
            var direction = callbackContext.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnInteract(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                _hero.Interact();
            }
        }

        public void OnAttack(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                _hero.Attack();
            }
        }

        public void OnThrow(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                _hero.Throw();
            }
        }

        public void OnUseItem(InputAction.CallbackContext callbackContext)
        {
            if (callbackContext.performed)
            {
                Debug.Log("Its HeroInputReader");
                _hero.UsePotion();
            }
        }
    }
}
