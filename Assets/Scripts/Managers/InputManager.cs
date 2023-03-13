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
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        m_PlayerInput = GetComponent<PlayerInput>();
    }

    public void AddPerform(string name, Action<InputAction.CallbackContext> func)
    {
        instance.m_PlayerInput.actions[name].performed += func;
    }
    public void RemovePerform(string name, Action<InputAction.CallbackContext> func)
    {
        instance.m_PlayerInput.actions[name].performed -= func;
    }
}
