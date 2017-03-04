using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPPT : MonoBehaviour {

    public GameObject model;

    // the working controller
    public GameObject thisController;
    int controllerIndex;

    // the image urls for ppt
    private String[] urls;
    // the index of currently showing image
    private int index;

    public GameObject display;

    // display control flags
    private Boolean clicked;    
    private Boolean right;

    private String numSlidesURL = "http://ec2-54-67-100-135.us-west-1.compute.amazonaws.com/numslides";

    private int numSlides;

    private int startSlide;
    private int endSlide;

    void Start()
    {
        model.SetActive(false);

        //_controller = GetComponent<SteamVR_TrackedController>();
        
        index = -1;
  
        clicked = false;
        right = true;
        
        StartCoroutine(init());

        // update changing once per second
        InvokeRepeating("change", 0, 0.8f);

    }

    IEnumerator init()
    {
        WWW numSlidesString = new WWW(numSlidesURL);
        yield return numSlidesString;
        

        numSlides = int.Parse(numSlidesString.text);

        Debug.Log(numSlides);

        urls = new String[numSlides];

        //get the interval for the model
        startSlide = 20;
        endSlide = 35;

        for (int i = 0; i< numSlides; i++)
        {
            urls[i] = "http://ec2-54-67-100-135.us-west-1.compute.amazonaws.com/img/Slide"+(i+1)+".jpg";
            Debug.Log(urls[i]);
        } 

        StartCoroutine(showPPT());
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

        // change model state

        if(index >= startSlide && index <= endSlide)
        {
            model.SetActive(true);
        }
        else
        {
            model.SetActive(false);
        }

    }

    void Update()
    {
        
        // get which controller is working
        controllerIndex = (int)thisController.GetComponent<SteamVR_TrackedObject>().GetDeviceIndex();

        
        if (SteamVR_Controller.Input(controllerIndex).GetPress(SteamVR_Controller.ButtonMask.Axis0) && !clicked )
        {
            Debug.Log("pressed");

            // check if the user is pressing on the left side of pad or the right side
            if( SteamVR_Controller.Input(controllerIndex).GetAxis().x < 0.0f )
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


    IEnumerator showPPT()
    {


        if (index < urls.Length  )
        {

            //Debug.Log("left or right: "+right);
            //Debug.Log("..." + clicked);

            // going to the next silde if the last condition of user pressing is right
            // do not change if it is the last slide
            if (right && index < urls.Length - 1)
                {
                    index++;
                    //Debug.Log("...//" + clicked);
            }

            // go to the previous slide
            else if(!right)
                {
                    if (index != 0)
                    {
                        index--;                      
                    }
                }

            }


        //Debug.Log(index);
        String url = urls[index];
        
        // get the url of the image
        WWW www = new WWW(url);
        print(index+"....");
        yield return www;

        //Debug.Log("showPPt");

        //update the display screen using the image as the texture
        display.GetComponent<Renderer>().material.mainTexture = www.texture;
       
    } 
}
