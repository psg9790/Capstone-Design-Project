using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
public class MAP_interact : MonoBehaviour
{
    public GameObject Map_Select;
    
    public GameObject Player;

    private Move PlayerMove;

    private NavMeshAgent NavOnOff;

    public string SceneName;

    private bool isEnter;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerMove = Player.GetComponent<Move>();
        NavOnOff = Player.GetComponent<NavMeshAgent>();
        isEnter = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isEnter && Input.GetKeyDown(KeyCode.F))
        {
            Player.GetComponent<Move>().enabled = false;
            Player.GetComponent<NavMeshAgent>().enabled = false;
            Debug.Log("portal click");
            Map_Select.SetActive(true);   
        }
        
        
    }

     void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isEnter = true;
        }
    }

    void OnTriggerExit()
    {
        isEnter = false;
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player")) 
        {
            Map_Select.SetActive(false);
        }
    }
    
    public void quit()
    {
        Map_Select.SetActive(false);
        Player.GetComponent<Move>().enabled = true;
        Player.GetComponent<NavMeshAgent>().enabled = true;
    }
    
    
    
    public void Move_Dun()
    {
        SceneManager.LoadScene(SceneName);
    }

    public void GetItem()
    {
        
    }
    
}
