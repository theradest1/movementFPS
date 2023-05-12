using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Look : MonoBehaviour
{
    ControlsManager controlsManager;
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

    [Header("cam shake")]
    public Vector3 initialPos;
    public float translationalMult;
    public float rotationalMult;
    public float camShakeDecay;
    public float amplitude;


    void Start()
    {
        player = GameObject.Find("Player");
        rb = player.GetComponent<Rigidbody>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        camRotX = 90;
        //Cursor.lockState = CursorLockMode.Locked;

        initialPos = transform.localPosition;
    }

    public void camShake(float amount){
        amplitude += amount;
    }   

    // Update is called once per frame
    void Update()
    {
        scopeCam.transform.rotation = Quaternion.LookRotation(scopeCam.transform.position - transform.position, transform.up);

        if(controlsManager.aiming){
            camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSenseADS, minCamRotX, maxCamRotX);
            rotY += controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSenseADS;
        }
        else{
            camRotX = Mathf.Clamp(camRotX - controlsManager.mouseDelta.y * LookSpeedVertical * generalSense, minCamRotX, maxCamRotX);
            rotY += controlsManager.mouseDelta.x * LookSpeedHorizontal * generalSense;
        }
        this.gameObject.transform.localRotation = Quaternion.Euler(camRotX, 0f, 0f);
        rb.MoveRotation(Quaternion.Euler(0f, rotY, 0f));
        
        transform.localPosition = Random.Range(-amplitude, amplitude) * transform.right * translationalMult + Random.Range(-amplitude, amplitude) * transform.up * translationalMult + initialPos;
    }

    void FixedUpdate() {
        amplitude *= camShakeDecay;
    }
}
