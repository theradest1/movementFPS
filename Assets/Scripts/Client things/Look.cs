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
        //Debug.Log(controlsManager.mouseDelta);
        //player.transform.Rotate(0f, controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense, 0f);

        camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSense, minCamRotX, maxCamRotX);
        this.gameObject.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
    }

    private void FixedUpdate() {
        rb.rotation *= Quaternion.Euler(0f, controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense, 0f);
    }
}
