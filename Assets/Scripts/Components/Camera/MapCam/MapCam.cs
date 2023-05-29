using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCam : MonoBehaviour
{
    private Camera cam;
    private Canvas mapCanvas;

    private void Start()
    {
        cam = GetComponent<Camera>();
        mapCanvas = GetComponentInChildren<Canvas>(true);
        mapCanvas.gameObject.SetActive(false);
        if (InputManager.Instance != null) // 탭 클릭/취소 이벤트 등록
        {
            InputManager.Instance.AddPerformed(InputKey.Tab, EnableMap);
            InputManager.Instance.AddCanceled(InputKey.Tab, DisableMap);
        }
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null) // 이벤트 해제
        {
            InputManager.Instance.RemovePerformed(InputKey.Tab, EnableMap);
            InputManager.Instance.RemoveCanceled(InputKey.Tab, DisableMap);
        }
    }

    private Coroutine OnEnabledCo;

    void EnableMap(InputAction.CallbackContext context)
    {
        OnEnabledCo = StartCoroutine(OnEnabledIE());
        mapCanvas.gameObject.SetActive(true);
    }

    void DisableMap(InputAction.CallbackContext context)
    {
        if (OnEnabledCo != null)
        {
            StopCoroutine(OnEnabledCo);
        }

        mapCanvas.gameObject.SetActive(false);
    }

    IEnumerator OnEnabledIE()
    {
        while (true)
        {
            if (Player.Instance != null)
            {
                transform.position = Player.Instance.transform.position + Vector3.up * 50f;
            }
            yield return null;
            float delta = InputManager.Instance.GetAction(InputKey.MouseWheel).ReadValue<float>();
            // if (delta > 0)
            // {
            //     UnityEngine.Debug.Log("wheel up");
            // }
            // else if (delta < 0)
            // {
            //     UnityEngine.Debug.Log("wheel down");
            // }
            // UnityEngine.Debug.Log(delta);
            cam.orthographicSize = Math.Clamp(cam.orthographicSize - delta * 0.01f, 0, 100);
        }
    }
}