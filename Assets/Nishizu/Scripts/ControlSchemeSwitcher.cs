using UnityEngine;
using UnityEngine.InputSystem;

public class ControlSchemeSwitcher : MonoBehaviour
{
    public PlayerInput playerInput;

    void Start()
    {
        // 初期状態ではKeyboard & Mouseを使用
        SwitchControlScheme(DeviceType.KeyboardAndMouse);
    }

    void Update()
    {
        // 現在接続されているデバイスに基づいてコントロールスキームを切り替える
        if (Keyboard.current != null)
        {
            SwitchControlScheme(DeviceType.KeyboardAndMouse);
        }
        else if (Gamepad.current != null)
        {
            SwitchControlScheme(DeviceType.Gamepad);
        }
    }

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

    // デバイスの種類を示すための列挙型
    enum DeviceType
    {
        KeyboardAndMouse,
        Gamepad
    }
}
