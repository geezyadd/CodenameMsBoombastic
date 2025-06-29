using System;
using UnityEngine;

namespace InputModule {
    public interface IInputListener : InputActions.IPlayerActions {
        public event Action<Vector2> Move;

        public void Enable();
        public void Disable();
        public void EnablePlayerAction();
        public void DisablePlayerAction();
    }
}