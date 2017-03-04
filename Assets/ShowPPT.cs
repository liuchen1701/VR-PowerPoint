using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShowPPT : MonoBehaviour {

    public GameObject model;

    private int startSlide;
    private int endSlide;

    // the image urls for ppt
    public String[] urls;
    // the index of currently showing image

    public GameObject display;

    private String numSlidesURL = "http://ec2-54-67-100-135.us-west-1.compute.amazonaws.com/numslides";

    private int numSlides;

    void Start()
    {
        model.SetActive(false);

        //_controller = GetComponent<SteamVR_TrackedController>();
                
        StartCoroutine(init());
        

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

    }

    

    


    IEnumerator changeSlide(int index)
    {

        //Debug.Log(index);
        String url = urls[index];
        
        // get the url of the image
        WWW www = new WWW(url);
       
        yield return www;

        //Debug.Log("showPPt");

        //update the display screen using the image as the texture
        display.GetComponent<Renderer>().material.mainTexture = www.texture;

        // change model state
        if (index >= startSlide && index <= endSlide)
        {
            model.SetActive(true);
        }
        else
        {
            model.SetActive(false);
        }
    } 

    

    [ClientRpc]
    public void RpcSyncPPT(int index)
    {

        Debug.Log("the current ppt number: " + index);

        StartCoroutine(changeSlide(index));
    }
}
