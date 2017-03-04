using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

public class DisplayModels : MonoBehaviour
{
    public GameObject models;

    private List<GameObject> modelList = new List<GameObject>();
    private int i = 0;

    private Vector2 touchPos;
    private Vector2 beginTouchPos;
    private Vector2 endTouchPos;
    private Vector2 lastTouchPos;

    private bool scaleOn = false;

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

        // Toggle displaying models
        if ( ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger) )
        {
            ToggleDisplay();
        }

        // Toggle scaling
        if ( ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Pad) )
        {
            ToggleScale();
        }

        
        if ( ViveInput.GetPress(HandRole.RightHand, ControllerButton.PadTouch) )
        {
            RecordTouch();
            offset = beginTouchPos - lastTouchPos;

            touchPos = ViveInput.GetPadAxis(HandRole.RightHand);

            // Scale the model bigger or smaller if swiping up or down on touchpad
            if (scaleOn)
            {
                Scale(offset);
            }

            // Rotate the model based on touchpad position
            else
            {
                Rotate(touchPos);
            }
        }

        // Switch among models in list if swiping left or right on touchpad
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

    void ToggleScale()
    {
        if (scaleOn)
        {
            scaleOn = false;
        }

        else
        {
            scaleOn = true;
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

    void Scale(Vector2 offset)
    {
        if (Mathf.Abs(offset.x) < 0.1f && offset.y < -0.1f)
        {
            modelList[i].transform.localScale += new Vector3(0.05f, 0.05f, 0.05f);
        }
        else if (Mathf.Abs(offset.x) < 0.1f && offset.y > 0.1f)
        {
            modelList[i].transform.localScale -= new Vector3(0.05f, 0.05f, 0.05f);
        }
    }

    void Rotate(Vector2 touchPos)
    {
        /*
        if (touchPos.x > 0.0f)
        {
            modelList[i].transform.Rotate( new Vector3(1.0f, 0.0f, 0.0f) );
        }
        else if (touchPos.x < 0.0f)
        {
            modelList[i].transform.Rotate( new Vector3(-1.0f, 0.0f, 0.0f) );
        }

        if (touchPos.y > 0.0f)
        {
            modelList[i].transform.Rotate( new Vector3(0.0f, 0.0f, 1.0f) );
        }
        else if (touchPos.y < 0.0f)
        {
            modelList[i].transform.Rotate( new Vector3(0.0f, 0.0f, -1.0f) );
        }
        */

        modelList[i].transform.Rotate(new Vector3(1.0f * touchPos.x, 0.0f, 1.0f * touchPos.y));
    }

    void RecordTouch()
    {
        if ( ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.PadTouch) )
        {
            beginTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
            lastTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
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
