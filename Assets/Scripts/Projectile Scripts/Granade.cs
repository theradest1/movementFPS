using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float timeToExplode;
    float damage;
    public Rigidbody rb;
    public float radius;
    int senderID;
    ServerEvents serverEvents;
    PlayerManager playerManager;
    ServerComm serverComm;

    SoundManager soundManager;
    public int bangSound;
    public int bounceSound;

    GameObject playerCam;
    public LayerMask stopFrom;

    public void setInfo(Vector3 givenVelocity, float givenDamage, PlayerManager givePlayerManager, GameObject givenPlayerCam, int givenSenderID, ServerEvents givenServerEvents, ServerComm givenServerComm, SoundManager givenSoundManager){
        playerManager = givePlayerManager;
        serverEvents = givenServerEvents;
        soundManager = givenSoundManager;
        senderID = givenSenderID;
        playerCam = givenPlayerCam;
        serverComm = givenServerComm;
        damage = givenDamage;
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
    }

    void explode(){
        soundManager.playSound(bangSound, transform.position, 1f, 1f);
        if(!Physics.Raycast(transform.position, playerCam.transform.position - transform.position, Vector3.Distance(transform.position, playerCam.transform.position), stopFrom)){
            if(Vector3.Distance(transform.position, playerCam.transform.position) <= radius){
                if(playerManager.health > damage){
                    serverEvents.sendEventFromOther(senderID, "ue", "d", serverComm.ID + "~" + damage);
                }
                else{
                    serverEvents.sendEventFromOther(senderID, "ue", "death", serverComm.ID + "");
                }
            }
        }
        //Collider coll = cols[i];
        //if(!Physics.Raycast(transform.position, playerCam.transform.position - transform.position, Vector3.Distance(transform.position, playerCam.transform.position), stopFrom)){
        //    if(coll.gameObject.layer == 3){
        //        if(playerManager.health > damage){
        //            serverEvents.sendEvent("ue", "damage", serverComm.ID + "~" + damage);
        //        }
        //        else{
        //            serverEvents.sendEvent("ue", "death", serverComm.ID + "");
        //        }
        //    }
        //}
        Destroy(this.gameObject);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, radius);
    }

    void OnCollisionEnter(Collision coll){
        soundManager.playSound(bounceSound, transform.position, 1f, 2f);
    }
}
