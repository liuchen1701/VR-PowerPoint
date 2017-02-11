using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPPT : MonoBehaviour {

    public String[] urls = new String[1];
    
    public GameObject display;
    public SteamVR_TrackedController _controller;


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
        urls[0] = ("http://i.imgur.com/aSV44JT.png");
    }


    // Update is called once per frame
    void Update()
    {
        
        if (_controller.triggerPressed)
        {
            Debug.Log("111");
            StartCoroutine("showPPT");
            Debug.Log("2222");
        }
    }



    IEnumerator showPPT()
    {
        //urls[0] = ("http://i.imgur.com/aSV44JT.png");
        String url = urls[0];
        WWW www = new WWW(url);
        yield return www;
        Debug.Log("...");
        display.GetComponent<Renderer>().material.mainTexture = www.texture;
    }
}
