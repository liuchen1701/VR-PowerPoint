using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{
    public GameObject laser;
    public Vector3 startPoint;
    public Vector3 endPoint;
    private GameObject reticle;

	void Start ()
    {
        reticle = GameObject.CreatePrimitive(PrimitiveType.Sphere);
    }

    void Update ()
    {
        createLaser();
        updateReticle();
	}

    void updateReticle()
    {
        reticle.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        reticle.transform.localPosition = endPoint;
        reticle.GetComponent<Renderer>().material.color = Color.red;
    }

    void createLaser()
    {
        laser.GetComponent<LineRenderer>().SetPosition(0, startPoint);
        laser.GetComponent<LineRenderer>().SetPosition(1, endPoint);
        laser.GetComponent<LineRenderer>().startWidth = 0.01f;
        laser.GetComponent<LineRenderer>().endWidth = 0.01f;
        laser.GetComponent<LineRenderer>().material.color = new Color(1.0f, 0f, 0f, 0.75f); ;
    }
}
