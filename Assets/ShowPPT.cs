using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPPT : MonoBehaviour {


    GameObject thisController;
    int controllerIndex;
    private String[] urls = new String[4];
    
    public GameObject display;


    private Boolean clicked;
    private int index;
    private Boolean right;

    // Use this for initialization
    /*IEnumerator Start()
    {
        urls[0]=("http://i.imgur.com/aSV44JT.png");
        String url = urls[0];
        WWW www = new WWW(url);
        yield return www;
        Debug.Log("...");
        display.GetComponent<Renderer>().material.mainTexture = www.texture;
    }*/
    void Start()
    {
        controllerIndex = (int)thisController.GetComponent<SteamVR_TrackedObject>().GetDeviceIndex();
        //_controller = GetComponent<SteamVR_TrackedController>();
        urls[0] = ("http://i.imgur.com/BqoNR9k.jpg");
        urls[1] = ("http://i.imgur.com/BqQsNge.jpg");
        urls[2] = ("http://i.imgur.com/6eEYNbO.jpg");
        urls[3] = ("http://i.imgur.com/ctg191V.jpg");
        index = 0;
  
        clicked = false;
    }



    void Update()
    {
        if(SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Axis0) )
        {
            if( SteamVR_Controller.Input(controllerIndex).GetAxis().x < 0.0f )
            {
                right = false;
            }
            else
            {
                right = true;
            }
            clicked = true;
        }
        if(!SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Axis0))
        {
            clicked = false;
            
                StartCoroutine("showPPT");
            
        }
    }


    IEnumerator showPPT()
    {

        if (index < urls.Length - 1 && index != 0)
        {
            if (right)
            {
                index++;
            }
            else
            {
                index--;
            }

        }

        String url = urls[index];
        
        WWW www = new WWW(url);
        
        yield return www;

        display.GetComponent<Renderer>().material.mainTexture = www.texture;

        

    }

  
}
