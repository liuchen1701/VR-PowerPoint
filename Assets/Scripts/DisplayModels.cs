using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class DisplayModels : MonoBehaviour
{
    public GameObject models;

    private List<GameObject> modelList = new List<GameObject>();
    private int i = 0;
    private bool swipedLeft = false, swipedRight = false;
    private Vector2 beginTouchPos;
    private Vector2 endTouchPos;
    private Vector2 lastTouchPos;

    void Start()
    {
        foreach (Transform child in models.transform)
        {
            modelList.Add(child.gameObject);
            child.gameObject.SetActive(false);
        }

        print(modelList.Count);
    }

    void Update()
    {
        Vector2 offset;

        // Record touch down position
        if ( ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.PadTouch) )
        {
            beginTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
            lastTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
        }

        // Toggle displaying models
        if ( ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger) )
        {
            ToggleDisplay();
        }

        //scale the model bigger or smaller if swiping up or down on touchpad
        if ( ViveInput.GetPress(HandRole.RightHand, ControllerButton.PadTouch) )
        {
            offset = ViveInput.GetPadAxis(HandRole.RightHand) - lastTouchPos;
            if (Mathf.Abs(offset.x) < 0.1f && offset.y < -0.1f)
            {
                modelList[i].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
            }
            else if (Mathf.Abs(offset.x) < 0.1f && offset.y > 0.1f)
            {
                modelList[i].transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
            }
        }

        //switch among models in list if swiping left or right on touchpad
        if ( ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.PadTouch) )
        {
            endTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
            lastTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
            offset = endTouchPos - beginTouchPos;
            if (offset.x < -0.1f && Mathf.Abs(offset.y) < 0.1f)
            {
                DisplayNext();
            }
            else if (offset.x > 0.1f && Mathf.Abs(offset.y) < 0.1f)
            {
                DisplayPrev();
            }
        }
    }

    void ToggleDisplay()
    {
        if (modelList[i].active)
        {
            modelList[i].SetActive(false);
        }
        else
        {
            modelList[i].SetActive(true);
        }
    }

    void DisplayNext()
    {
        //deactivate the current one
        modelList[i].SetActive(false);

        //activate the next one and update index
        if (i + 1 == modelList.Count)
        {
            i = 0;
            modelList[i].SetActive(true);
        }
        else
        {
            i++;
            modelList[i].SetActive(true);
        }
    }

    void DisplayPrev()
    {
        //deactivate the current one
        modelList[i].SetActive(false);

        //activate the previous one and update index
        if (i == 0)
        {
            i = modelList.Count - 1;
            modelList[i].SetActive(true);
        }
        else
        {
            i--;
            modelList[i].SetActive(true);
        }
    }
}
