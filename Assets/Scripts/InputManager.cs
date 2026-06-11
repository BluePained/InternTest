using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputSystem_Actions InputActions = new InputSystem_Actions();

    public static event Action<InputActionMap> ActionMapChanged;

    public static void ToggleActionMap(InputActionMap actionMap)
    {
        if (actionMap.enabled) return;

        InputActions.Disable();
        ActionMapChanged?.Invoke(actionMap);
        actionMap.Enable();
    }
}
