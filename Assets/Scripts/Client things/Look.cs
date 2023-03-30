using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    public float LookSpeedHorizontal;
    public float LookSpeedVertical;
    public float generalSense;
    public float generalSenseADS;
    public float camRotX;
    public float rotY = 0f;
    public float minCamRotX;
    public float maxCamRotX;

    public GameObject scopeCam;


    ControlsManager controlsManager;

    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        camRotX = 90;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //scopeCam.transform.localRotation = transform.localRotation;
        scopeCam.transform.rotation = Quaternion.LookRotation(scopeCam.transform.position - transform.position, transform.up);
        //Debug.Log(controlsManager.mouseDelta);
        //player.transform.Rotate(0f, controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense, 0f);
        if(controlsManager.aiming){
            camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSenseADS, minCamRotX, maxCamRotX);
        }
        else{
            camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSense, minCamRotX, maxCamRotX);
        }
        this.gameObject.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
        
        if(controlsManager.aiming){
            rotY += controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSenseADS;
            //rb.rotation *= Quaternion.Euler(0f, controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSenseADS, 0f);
        }
        else{
            rotY += controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense;
        }
        rb.MoveRotation(Quaternion.Euler(0f, rotY, 0f));
    }
}
