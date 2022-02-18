namespace FireChickenGames.Combat.Core.PlayerInput
{
    using System;
    using UnityEngine;

    [Serializable]
    public class UserInputKey
    {
        public bool useMouseButton = false;
        public MouseButton mouseButton = MouseButton.Left;
        public KeyCode keyCode = KeyCode.A;

        #region Public Property API
        public KeyCode FinalKeyCode => useMouseButton ? (KeyCode)mouseButton : keyCode;
        public bool IsHeld => Input.GetKey(FinalKeyCode);
        public bool IsUp => Input.GetKeyUp(FinalKeyCode);
        public bool IsDown => Input.GetKeyDown(FinalKeyCode);
        #endregion

        public UserInputKey(MouseButton mouseButton)
        {
            this.mouseButton = mouseButton;
            useMouseButton = true;
        }

        public UserInputKey(KeyCode keyCode)
        {
            this.keyCode = keyCode;
            useMouseButton = false;
        }
    }
}