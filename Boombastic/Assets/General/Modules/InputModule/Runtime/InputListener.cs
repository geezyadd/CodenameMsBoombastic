using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputModule {
    public class InputListener : MonoBehaviour, IInputListener {
        private InputActions _inputActions;
        private InputActions.PlayerActions _playerActions;
        public event Action<Vector2> Move;

        public void Awake() {
            _inputActions = new InputActions();
            _playerActions = new InputActions.PlayerActions(_inputActions);
            _playerActions.AddCallbacks(this);
            _inputActions.Enable();
        }

        public void OnDestroy() {
            _playerActions.RemoveCallbacks(this);
            _inputActions.Disable();
        }

        public void Enable() =>
            _inputActions.Enable();

        public void Disable() =>
            _inputActions.Disable();

        public void EnablePlayerAction() =>
            _playerActions.Enable();

        public void DisablePlayerAction() =>
            _playerActions.Disable();

        public void OnMove(InputAction.CallbackContext context) {
            Vector2 readValue = context.ReadValue<Vector2>();
            Move?.Invoke(readValue);
        }
    }
}