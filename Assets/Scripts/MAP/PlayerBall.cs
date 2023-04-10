using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerBall : MonoBehaviour
{
    public float jumpnum=40;

    Rigidbody rigid;
    //2단점프 방지
    bool isJump;

    public int itemcount;

    void Awake(){
        isJump= false;
        rigid = GetComponent<Rigidbody>();
    }
    void Update() {
       
    }
    void FixedUpdate() {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        rigid.AddForce(new Vector3(h,0,v), ForceMode.Impulse);
        
    }


}
