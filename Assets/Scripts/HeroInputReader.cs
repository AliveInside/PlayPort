using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayPort
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Hero _hero;

        // private void Update()
        // {
            // Legacy method #2
            // var horizontal = Input.GetAxis("Horizontal");
            // _hero.SetDirection(horizontal);

            // Legacy method #1
            // if(Input.GetKey(KeyCode.A))
            // {
            //     _hero.SetDirection(-1);
            // }
            // else if(Input.GetKey(KeyCode.D))
            // {
            //     _hero.SetDirection(1);
            // }
            // else
            // {
            //     _hero.SetDirection(0);
            // }
        // }


        // InputManager Method 1(Invoke Unity Events)
        // public void OnHorizontalMovement(InputAction.CallbackContext callbackContext)
        // {
        //     var direction = callbackContext.ReadValue<float>();
        //     _hero.SetDirection(direction);
        // }

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




        // InputManager Method 2(Broadcast messages)
        // private void OnHorizontalMovement(InputValue value)
        // {
        //     if value.

        //     var direction = value.Get<float>();
        //     _hero.SetDirection(direction);
        // }

        // private void OnSpeech(InputValue value)
        // {
        //     _hero.Speech();
        // }


        // InputManager Method 3()
        // private HeroInputAction _inputAction;
        // private void Awake()
        // {
        //     _inputAction = new HeroInputAction();
        //     _inputAction.Hero.HorizontalMovement.performed += OnHorizontalMovement;
        //     _inputAction.Hero.HorizontalMovement.canceled += OnHorizontalMovement;
        //     _inputAction.Hero.Speech.performed += OnSpeech;
        // }

        // private void OnEnable()
        // {
        //     _inputAction.Enable();
        // }

        // public void OnHorizontalMovement(InputAction.CallbackContext callbackContext)
        // {
        //     var direction = callbackContext.ReadValue<float>();
        //     _hero.SetDirection(direction);
        // }

        // public void OnSpeech(InputAction.CallbackContext callbackContext)
        // {
        //     if(callbackContext.canceled)
        //     {
        //         _hero.Speech();
        //     }
        // }
    }
}
