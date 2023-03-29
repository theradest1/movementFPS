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
    public float camRotX;
    public float minCamRotX;
    public float maxCamRotX;

    public GameObject scopeCam;
    public float scopedSenseMult;


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
            camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSense * scopedSenseMult, minCamRotX, maxCamRotX);
        }
        else{
            camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSense, minCamRotX, maxCamRotX);
        }
        this.gameObject.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
    }

    private void FixedUpdate() {
        if(controlsManager.aiming){
            rb.rotation *= Quaternion.Euler(0f, controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense * scopedSenseMult, 0f);
        }
        else{
            rb.rotation *= Quaternion.Euler(0f, controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense, 0f);
        }
    }
}
