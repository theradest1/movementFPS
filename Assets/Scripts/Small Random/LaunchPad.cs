using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float strength = 10f;
    public float forwardStrength = 1f;
    public int launchSound1;
    public int launchSound2;
    public bool launchToGameObject;
    public GameObject target;
    Rigidbody playerRb;
    GameObject playerCam;
    ServerEvents serverEvents;
    movement movementScript;

    private void Start() {
        movementScript = GameObject.Find("Player").GetComponent<movement>();
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        playerCam = GameObject.Find("Main Camera");
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player"){
            if(launchToGameObject){
                //playerRb.gameObject.transform.position = transform.position + Vector3.up;
                playerRb.velocity = Vector3.zero;
                movementScript.launchTo(target.transform.position);
            }
            else{
                playerRb.velocity = new Vector3(playerRb.velocity.x, strength, playerRb.velocity.z);
                playerRb.velocity += playerCam.transform.forward * forwardStrength;
            }
            serverEvents.sendEvent("ue", "sound", launchSound1 + "~" + this.transform.position + "~1~1");
            serverEvents.sendEvent("ue", "sound", launchSound2 + "~" + this.transform.position + "~1~1");
        }
    }
}
