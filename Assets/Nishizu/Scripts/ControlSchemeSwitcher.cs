using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSchemeSwitcher : MonoBehaviour
{
    public PlayerInput playerInput;

    void Start()
    {
        SwitchControlScheme(DeviceType.KeyboardAndMouse);
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            SwitchControlScheme(DeviceType.KeyboardAndMouse);
        }
        else if (Gamepad.current != null)
        {
            SwitchControlScheme(DeviceType.Gamepad);
        }
    }
    /// <summary>
    /// デバイスを変える
    /// </summary>
    /// <param name="deviceType">に応じてデバイスを変える</param>
    void SwitchControlScheme(DeviceType deviceType)
    {
        if (deviceType == DeviceType.KeyboardAndMouse)
        {
            playerInput.SwitchCurrentControlScheme("Keyboard&Mouse");
        }
        else if (deviceType == DeviceType.Gamepad)
        {
            playerInput.SwitchCurrentControlScheme("Gamepad");
        }
    }
    enum DeviceType
    {
        KeyboardAndMouse,
        Gamepad
    }
}
