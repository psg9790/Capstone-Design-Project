using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public void LeaderBoard_ButtonClick()
    {
#if StovePCSDK

#else
    StovePCSDKManager.Instance.WriteLog("Stove 버전이 아닙니다.");
#endif
    }
}