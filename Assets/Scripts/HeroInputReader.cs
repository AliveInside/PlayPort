using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayPort
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        public void OnSpeech(InputAction.CallbackContext callbackContext)
        {
            if(callbackContext.canceled)
            {
                _hero.Speech();
            }
        }

        public void OnMovement(InputAction.CallbackContext callbackContext)
        {
            var direction = callbackContext.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }
    }
}
