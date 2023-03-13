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

    public static InputManager Instance
    {
        get { return instance; }
    }

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

    public void AddPerformed(InputType type, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[type.ToString()].performed += action;
    }
    public void RemovePerformed(InputType type, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[type.ToString()].performed -= action;
    }
    public void AddCanceled(InputType type, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[type.ToString()].canceled += action;
    }
    public void RemoveCanceled(InputType type, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[type.ToString()].canceled -= action;
    }
    public void AddStarted(InputType type, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[type.ToString()].started += action;
    }
    public void RemoveStarted(InputType type, Action<InputAction.CallbackContext> action)
    {
        instance.m_PlayerInput.actions[type.ToString()].started -= action;
    }
}

public enum InputType
{
    RightClick,
    LeftClick
}
