using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPPT : MonoBehaviour {


    public GameObject thisController;
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
        
        //_controller = GetComponent<SteamVR_TrackedController>();
        urls[0] = ("http://i.imgur.com/BqoNR9k.jpg");
        urls[1] = ("http://i.imgur.com/BqQsNge.jpg");
        urls[2] = ("http://i.imgur.com/6eEYNbO.jpg");
        urls[3] = ("http://i.imgur.com/ctg191V.jpg");
        index = -1;
  
        clicked = false;
        right = true;
        StartCoroutine("showPPT");
        InvokeRepeating("change", 0, 1);

    }

    void change()
    {
        if (!(SteamVR_Controller.Input(controllerIndex).GetPressDown(SteamVR_Controller.ButtonMask.Axis0)) && clicked)
        {
            print(clicked);
            if (clicked)
            {
                clicked = false;
                StartCoroutine("showPPT");

            }
        }
    }

    void Update()
    {

        //print(index);

        controllerIndex = (int)thisController.GetComponent<SteamVR_TrackedObject>().GetDeviceIndex();
        if (SteamVR_Controller.Input(controllerIndex).GetPress(SteamVR_Controller.ButtonMask.Axis0) && !clicked )
        {
            Debug.Log("pressed");
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
        
    }


    IEnumerator showPPT()
    {


        if (index < urls.Length  )
        {

            print("left or right: "+right);
                print("..." + clicked);
                if (right && index < urls.Length - 1)
                {
                    index++;                
                    print("...//" + clicked);
                }
                else if(!right)
                {
                    if (index != 0)
                    {
                        index--;                      
                    }
                }

            }
        

        print(index);
        String url = urls[index];
        
        WWW www = new WWW(url);
        print(index+"....");
        yield return www;
        print("showPPt");
        display.GetComponent<Renderer>().material.mainTexture = www.texture;

  
        

    }

  
}
