using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    public float strength = 10f;
    public float forwardStrength = 1f;
    public int launchSound1;
    public int launchSound2;
    Rigidbody playerRb;
    GameObject playerCam;
    ServerEvents serverEvents;

    private void Start() {
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        playerCam = GameObject.Find("Main Camera");
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player"){
            playerRb.velocity = new Vector3(playerRb.velocity.x, strength, playerRb.velocity.z);
            playerRb.velocity += playerCam.transform.forward * forwardStrength;
            serverEvents.sendEvent("ue", "sound", launchSound1 + "~" + this.transform.position + "~1~1");
            serverEvents.sendEvent("ue", "sound", launchSound2 + "~" + this.transform.position + "~1~1");
        }
    }
}
