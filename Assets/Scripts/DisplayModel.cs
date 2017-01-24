using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DisplayModel : MonoBehaviour {
	public GameObject models;
	private List<GameObject> modelList = new List<GameObject> ();
	private int i = 0;
	private bool swipeLeft = false, swipeRight = false;

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

		if(GvrController.ClickButtonDown) {

			if (modelList [i].active) {
				modelList [i].SetActive (false);
			} else {
				modelList [i].SetActive (true);
			}

			if (i - 1 < 0) {
				modelList [modelList.Count - 1].SetActive (false);
			} else {
				modelList [i - 1].SetActive (false);
			}
			modelList [i].SetActive (true);
			if (i + 1 == modelList.Count) {
				i = 0;
			} else {
				i++;
			}
		} else {



		}
	}
}
