using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class InputManager : MonoBehaviour
{
    static InputManager instance;
    private PlayerInput m_PlayerInput;
    private InputAction m_RightClick;
    private InputAction m_LeftClick;
    public static InputManager Instance
    {
        get { return instance; }
    }

    public InputAction RightClick
    {
        get { return m_RightClick; }
        set { }
    }
    public InputAction LeftClick
    {
        get { return m_LeftClick; }
        set { }
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
        m_RightClick = m_PlayerInput.actions["RightClick"];
        m_LeftClick = m_PlayerInput.actions["LeftClick"];
    }
}
