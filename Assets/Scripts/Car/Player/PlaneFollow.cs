using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneFollow : MonoBehaviour
{

    Transform plane;

    private void Start()
    {
        plane = GameObject.Find("Plane").GetComponent<Transform>();
    }

    void Update()
    {
        plane.position = new Vector3(transform.position.x,0f, transform.position.z);
    }
}
