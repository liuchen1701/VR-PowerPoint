using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayModel : MonoBehaviour {
	public GameObject models;
	private List<GameObject> modelList = new List<GameObject> ();
	private int i = 0;
	private bool swipedLeft = false, swipedRight = false;
	private Vector2 beginTouchPos;
	private Vector2 endTouchPos;
	private Vector2 lastTouchPos;

	// Use this for initialization
	void Start () {
		foreach (Transform child in models.transform) {
			modelList.Add (child.gameObject);
			child.gameObject.SetActive (false);
		}

		print (modelList.Count);
	}

	// Update is called once per frame
	void Update () {
		Vector2 offset;

		//record touch down position
		if(GvrController.TouchDown) {
			beginTouchPos = GvrController.TouchPos;
			lastTouchPos = GvrController.TouchPos;
		}

		//toggle displaying models or not
		if(GvrController.ClickButtonDown) {
			ToggleDisplay ();
		}

		//scale the model bigger or smaller if swiping up or down on touchpad
		if (GvrController.IsTouching) {
			offset = GvrController.TouchPos - lastTouchPos;
			if(Mathf.Abs(offset.x) < 0.1f && offset.y < -0.1f) {
				modelList [i].transform.localScale += new Vector3 (0.05f, 0.05f, 0.05f);
			} else if(Mathf.Abs(offset.x) < 0.1f && offset.y > 0.1f) {
				modelList [i].transform.localScale -= new Vector3 (0.05f, 0.05f, 0.05f);
			}
		}

		//switch among models in list if swiping left or right on touchpad
		if(GvrController.TouchUp) {
			endTouchPos = GvrController.TouchPos;
			lastTouchPos = GvrController.TouchPos;
			offset = endTouchPos - beginTouchPos;
			if(offset.x < -0.1f && Mathf.Abs(offset.y) < 0.1f) {
				DisplayNext ();
			} else if(offset.x > 0.1f && Mathf.Abs(offset.y) < 0.1f) {
				DisplayPrev ();	
			}
		}
	}

	void ToggleDisplay() {
		if (modelList [i].active) {
			modelList [i].SetActive (false);
		} else {
			modelList [i].SetActive (true);
		}
	}

	void DisplayNext() {
		//deactivate the current one
		modelList[i].SetActive(false);

		//activate the next one and update index
		if (i + 1 == modelList.Count) {
			i = 0;
			modelList [i].SetActive (true);
		} else {
			i++;
			modelList [i].SetActive (true);
		}
	}

	void DisplayPrev() {
		//deactivate the current one
		modelList[i].SetActive(false);

		//activate the previous one and update index
		if (i == 0) {
			i = modelList.Count - 1;
			modelList [i].SetActive (true);
		} else {
			i--;
			modelList [i].SetActive (true);
		}
	}
}
