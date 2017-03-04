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
    private Vector2 delta;

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
        if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.Trigger))
        {
            ToggleDisplay();
        }

        if (!ViveInput.GetPress(HandRole.RightHand, ControllerButton.Grip))
        {

            if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.Pad))
            {
                touchPos = ViveInput.GetPadPressAxis(HandRole.RightHand);
                if (touchPos.y > 0.0f)
                {
                    Scale(1);
                }
                else if (touchPos.y < 0.0f)
                {
                    Scale(-1);
                }
            }
            else if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.PadTouch))
            {
                print(delta.magnitude + " " + delta.ToString());
                if (delta.magnitude > 0.05f)
                {
                    if (delta.x < 0)
                    {
                        DisplayPrev();
                    }
                    else
                    {
                        DisplayNext();
                    }
                }
            }
            else if (!ViveInput.GetPadTouchDelta(HandRole.RightHand).Equals(new Vector2(0.0f, 0.0f)))
            {
                delta = ViveInput.GetPadTouchDelta(HandRole.RightHand);
            }


        }
        else
        {
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.PadTouch))
            {
                beginTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
                lastTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
            }

            // Switch among models in list if swiping left or right on touchpad
            if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.PadTouch))
            {
                endTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
                lastTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
                offset = endTouchPos - beginTouchPos;
            }

            if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.PadTouch))
            {
                lastTouchPos = ViveInput.GetPadAxis(HandRole.RightHand);
                touchPos = ViveInput.GetPadAxis(HandRole.RightHand);
                offset = touchPos - lastTouchPos;

                Rotate(touchPos);
                lastTouchPos = touchPos;
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

    void Scale(int direction)
    {
        if (direction >= 0)
        {
            modelList[i].transform.localScale += new Vector3(0.001f, 0.001f, 0.001f);
        }
        else
        {
            modelList[i].transform.localScale -= new Vector3(0.001f, 0.001f, 0.001f);
        }
    }

    void Rotate(Vector2 touchPos)
    {

        if (touchPos.x > 0.71f)
        {
            modelList[i].transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
        }
        else if (touchPos.x < -0.71f)
        {
            modelList[i].transform.Rotate(new Vector3(0.0f, -1.0f, 0.0f));
        }

        if (touchPos.y > 0.71f)
        {
            modelList[i].transform.Rotate(new Vector3(1.0f, 0.0f, 0.0f));
        }
        else if (touchPos.y < -0.71f)
        {
            modelList[i].transform.Rotate(new Vector3(-1.0f, 0.0f, 0.0f));
        }


        //modelList[i].transform.Rotate(new Vector3(1.0f * touchPos.y, 1.0f * touchPos.x, 0.0f));
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
