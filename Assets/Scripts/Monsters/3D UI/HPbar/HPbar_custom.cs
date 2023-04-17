using System;
using System.Collections;
using System.Collections.Generic;
using Monsters;
using Sirenix.OdinInspector;
using UnityEngine;

public class HPbar_custom : MonoBehaviour
{
    [SerializeField] private MeshRenderer red;
    [SerializeField] private MeshRenderer yellow;
    private Heart heart;
    private float prev_curhp;
    
    private float elapsedTime = 0;
    [SerializeField] private float endTime = 5f;

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (prev_curhp != heart.CUR_HP)
        {
            if (prev_curhp > heart.CUR_HP)
            {
                Red_Activity();
                Yellow_Activity();
            }

            elapsedTime = 0;
            prev_curhp = heart.CUR_HP;
        }

        if (elapsedTime > endTime)
        {
            Return();
        }
    }

    private void LateUpdate()
    {
        transform.position = heart.hpbar_pos.position;
    }

    private void Red_Activity()
    {
        float ratio = Math.Clamp(heart.CUR_HP / heart.MAX_HP, 0, 1);
        Vector3 red_scale = red.transform.localScale;
        red_scale.x = ratio;
        red.transform.localScale = red_scale;
        Vector3 red_pos = new Vector3(0, 0, -0.02f);
        red_pos.x = -((1 - ratio) / 2);
        red.transform.localPosition = red_pos;
    }

    private void Yellow_Activity()
    {
    }

    public void Activate(Heart _heart)
    {
        gameObject.SetActive(true);
        elapsedTime = 0;
        heart = _heart;
        transform.position = _heart.hpbar_pos.position;
        transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        prev_curhp = _heart.CUR_HP;
        Red_Activity();
    }

    public void Return()
    {
        heart.hpbar = null;
        heart = null;
        gameObject.SetActive(false);

        HPbar_pooling.Instance.Return_HPbar(this);
    }
}