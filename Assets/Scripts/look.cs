using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class look : MonoBehaviour
{
    GameObject player;
    public float lookSpeedHorizontal;
    public float lookSpeedVertical;
    public float generalSense;
    float camRotX = 0f;
    public float minCamRotX;
    public float maxCamRotX;


    public controlsManager controlsManagerScript;

    void Start()
    {
        player = GameObject.Find("Player");
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(controlsManagerScript.mouseDelta);
        player.transform.Rotate(0f, controlsManagerScript.mouseDelta.x * lookSpeedHorizontal * generalSense, 0f);

        camRotX = Mathf.Clamp(camRotX - controlsManagerScript.mouseDelta.y * lookSpeedVertical * generalSense, minCamRotX, maxCamRotX);
        this.gameObject.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
    }
}
