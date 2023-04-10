using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;
using System;

public class InputManager : MonoBehaviour
{
    static InputManager instance;
    private PlayerInput m_PlayerInput;
    public static InputManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            m_PlayerInput = GetComponent<PlayerInput>();
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            Debug.LogWarning("InputManager cannot be two : Deleted.");
        }
    }

    public void AddPerformed(InputKey key, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[key.ToString()].performed += action;
    }
    public void RemovePerformed(InputKey key, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[key.ToString()].performed -= action;
    }
    public void AddCanceled(InputKey key, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[key.ToString()].canceled += action;
    }
    public void RemoveCanceled(InputKey key, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[key.ToString()].canceled -= action;
    }
    public void AddStarted(InputKey key, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[key.ToString()].started += action;
    }
    public void RemoveStarted(InputKey key, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[key.ToString()].started -= action;
    }
    
    public Vector2 GetMousePosition()
    {
        return instance.m_PlayerInput.actions["MousePosition"].ReadValue<Vector2>();
    }

    public Vector2 GetWASD()
    {
        return instance.m_PlayerInput.actions["UDLR"].ReadValue<Vector2>();
    }

    public InputAction GetAction(InputKey _key)
    {
        return instance.m_PlayerInput.actions[_key.ToString()];
    }
}

public enum InputKey
{
    RightClick,
    LeftClick,
    SpaceClick,
    F
}
