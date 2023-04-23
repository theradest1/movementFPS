using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform objectToFollow;

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(1, objectToFollow.position);
        lineRenderer.SetPosition(0, transform.position);
        //transform.LookAt(objectToFollow.position);
    }
}
