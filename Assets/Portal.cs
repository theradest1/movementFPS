using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    Camera playerCam;
    [HideInInspector]
    public Camera portalCam;
    public Portal linkedPortal;

    void Start()
    {
        playerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        portalCam = this.transform.GetChild(0).GetComponent<Camera>();
    }

    void Update()
    {
        if (linkedPortal != null)
        {
            linkedPortal.portalCam.transform.rotation = Quaternion.LookRotation(playerCam.transform.position - portalCam.transform.position, Vector3.up);
        }
    }
}
