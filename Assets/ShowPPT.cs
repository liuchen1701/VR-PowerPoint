using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPPT : MonoBehaviour {

    private String[] urls = new String[4];
    
    public GameObject display;
    public SteamVR_TrackedController _controller;
    private Boolean canChange;
    private int index;

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
        index = 0;
        canChange = true;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (_controller.padPressed && canChange)
        {
            canChange = false;
            StartCoroutine("showPPT");
            
        }
    }



    IEnumerator showPPT()
    {
        
        String url = urls[index];
        
        WWW www = new WWW(url);
        yield return www;
        if (index < urls.Length -1)
        {
            index++;
        }
        
        canChange = true;
        display.GetComponent<Renderer>().material.mainTexture = www.texture;
    }
}
