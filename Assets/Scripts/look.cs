using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class look : MonoBehaviour
{
    public GameObject player;
    public float lookSpeedHorizontal;
    public float lookSpeedVertical;
    public float camRotX = 0f;
    public float minCamRotX;
    public float maxCamRotX;


    public controlsManager controlsManagerScript;

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(controlsManagerScript.mouseDelta);
        player.transform.Rotate(0f, controlsManagerScript.mouseDelta.x * lookSpeedHorizontal, 0f);

        camRotX = Mathf.Clamp(camRotX - controlsManagerScript.mouseDelta.y * lookSpeedVertical, minCamRotX, maxCamRotX);
        this.gameObject.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
    }
}
