using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;



public class ShowPPT_Vive : MonoBehaviour {

    public GameObject thisController;

    int controllerIndex;

    private int index;

    public GameObject model;





    // display control flags
    private Boolean clicked;
    private Boolean right;

    // Use this for initialization
    void Start () {
        clicked = false;
        right = true;
        InvokeRepeating("change", 0, 0.8f);

        index = -1;
        StartCoroutine("updateSlide");
    }
	


    void Update()
    {

        // get which controller is working
        controllerIndex = (int)thisController.GetComponent<SteamVR_TrackedObject>().GetDeviceIndex();


        if (SteamVR_Controller.Input(controllerIndex).GetPress(SteamVR_Controller.ButtonMask.Axis0) && !clicked)
        {
            Debug.Log("pressed");

            // check if the user is pressing on the left side of pad or the right side
            if (SteamVR_Controller.Input(controllerIndex).GetAxis().x < 0.0f)
            {
                right = false;
            }
            else
            {
                right = true;
            }
            // update the click condition, prepare for change
            clicked = true;
        }

    }

    void change()
    {
        if (!(SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) && clicked)
        {
            print(clicked);
            if (clicked)
            {
                clicked = false;
            }
        }


       

    }

    public void updateSlide()
    {
        if (index < this.GetComponent<ShowPPT>().urls.Length)
        {

            //Debug.Log("left or right: "+right);
            //Debug.Log("..." + clicked);

            // going to the next silde if the last condition of user pressing is right
            // do not change if it is the last slide
            if (right && index < this.GetComponent<ShowPPT>().urls.Length - 1)
            {
                index++;
                //Debug.Log("...//" + clicked);
            }

            // go to the previous slide
            else if (!right)
            {
                if (index != 0)
                {
                    index--;
                }
            }
        }
        this.GetComponent<ShowPPT>().RpcSyncPPT(index);
    }
}
