using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MKdemoCtrl : MonoBehaviour {

    public GameObject CrownItem;
    public GameObject ShoulderItem;
    public GameObject MKBarItem;    

    public int currentAnim = 0;
    public Text AnimDisplayed;

    public Toggle tCrown;
    public Toggle tShoulder;
    public Toggle tMKBar;

    public Animator MKanimator;
    public string[] animationList;


    // Use this for initialization
    void Start () {

        //Find Mk props at the Scene.
        CrownItem = GameObject.Find("Mking_Crown");
        ShoulderItem = GameObject.Find("low_shoulder");
        MKBarItem= GameObject.Find("MKing_Bar");
        MKanimator = gameObject.GetComponent<Animator>();
        AnimDisplayed.text = animationList[currentAnim];
    }
	
	// Update is called once per frame
	void Update () {
        
        ChangeShoulderItemState();
        ChangeMKBarItemState();
    }


    public void pickNextAnimation() {

        if (currentAnim <=11) {
            currentAnim++;
        }
        else
        {
            currentAnim = 0;
        }

        MKanimator.CrossFade(animationList[currentAnim], 0f);
        AnimDisplayed.text = animationList[currentAnim];
    }


    public void pickPrevAnimation()
    {

        if (currentAnim > 0)
        {
            currentAnim--;
        }
        else
        {
            currentAnim = 12;
        }

        MKanimator.CrossFade(animationList[currentAnim], 0f);
        AnimDisplayed.text = animationList[currentAnim];
    }


  
    
    
    //Change The visibility property of the Crown.
   public void CheckCrownItemState() {

        if (tCrown.isOn)
        {
            CrownItem.SetActive(true);
          
        }
        else
        {
            CrownItem.SetActive(false);
        }
    }

    //Change The visibility property of the Shoulder.
    void ChangeShoulderItemState() {


        if (tShoulder.isOn)
        {
            ShoulderItem.SetActive(true);
        }
        else
        {
            ShoulderItem.SetActive(false);
        }
    }

    //Change The visibility property of the MkBar.
    void ChangeMKBarItemState()
    {


        if (tMKBar.isOn)
        {
            MKBarItem.SetActive(true);
        }
        else
        {
            MKBarItem.SetActive(false);
        }
    }


}
