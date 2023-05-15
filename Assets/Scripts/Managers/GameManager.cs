using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 정확한 역할은 정해지지 않았음, 대신 GameManager로써 마스터 설정같은거 다 때려 넣을듯
// 일단 저장로직 인터페이스, 씬 이동할때 갈아끼울 씬로드매니저
public class GameManager : MonoBehaviour
{
    // singleton
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public GameObject menuSet;
    public GameObject invenSet;
    
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

        Application.targetFrameRate = 60;
    }

    void Update()
    {
        //Sub Menu
        if (Input.GetButtonDown("Cancel") && (invenSet.activeSelf)==false)
        {
            if (menuSet.activeSelf)
            {
                Time.timeScale = 1;
                menuSet.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                menuSet.SetActive(true); 
            }
        }
    }


}
