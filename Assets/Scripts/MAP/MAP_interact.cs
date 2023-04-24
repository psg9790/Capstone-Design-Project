using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MAP_interact : MonoBehaviour
{
    public GameObject Map_Select;
    public GameObject Map_Growth;
    public GameObject Map_Record;

    public string Growth_Dun;

    public string Record_Dun;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // 성장던전 선택
    public void Select_Growth()
    {
        Map_Select.SetActive(false);
        Map_Growth.SetActive(true);
    }
    
    // 던전 선택창으로 되돌아가기
    public void Return_Select()
    {
        if (Map_Growth.activeSelf == true){
            Map_Growth.SetActive(false);
        }else if (Map_Record.activeSelf == true) {
            Map_Record.SetActive(false);
        }
        Map_Select.SetActive(true);
    }
    
    // 기록던전 선택
    public void Select_Record()
    {
        Map_Select.SetActive(false);
        Map_Record.SetActive(true);
    }
    
    //던전 선택 종료
    public void Off_Select()
    {
        Map_Select.SetActive(false);
    }

    public void Move_Growth_Dun()
    {
        SceneManager.LoadScene(Growth_Dun);
    }

    public void Move_Record_Dun()
    {
        SceneManager.LoadScene(Record_Dun);
    }
}
