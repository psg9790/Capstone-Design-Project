using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerMove : MonoBehaviour
{
    Transform PlayerTransform;
    Vector3 Offset;
    void Start()
    {
        PlayerTransform= GameObject.FindGameObjectWithTag("Player").transform;
        Offset= transform.position - PlayerTransform.position;
    }
    
    void LateUpdate()
    {
        // 업데이트 연산 후 뒤따라가는 것으로 last업데이트 사용
        transform.position= PlayerTransform.position+Offset;
    }

}
