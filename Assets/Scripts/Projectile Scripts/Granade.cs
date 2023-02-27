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

    ProjectileFunctions projectileFunctions;

    public int bangSound;
    public int bounceSound;

    public GameObject distanceVisualizer;
    public GameObject explosionParticles;
    public LayerMask stopFrom;

    public void setInfo(Vector3 givenVelocity, float givenDamage, int givenSenderID, ProjectileFunctions givenProjectileFunctions){
        projectileFunctions = givenProjectileFunctions;
        senderID = givenSenderID;
        damage = givenDamage;
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
        distanceVisualizer.transform.localScale = new Vector3(radius * 2 / transform.localScale.x, .1f, radius * 2 / transform.localScale.x);
    }

    private void Update() {
        distanceVisualizer.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    void explode(){
        Destroy(Instantiate(explosionParticles, transform.position, Quaternion.identity), 5f);
        projectileFunctions.soundManager.playSound(bangSound, transform.position, 1f, 1f);
        if(!Physics.Raycast(transform.position, projectileFunctions.playerCam.transform.position - transform.position, Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position), stopFrom)){
            if(Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position) <= radius){
                damage *= 1 - Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position)/radius;
                if(projectileFunctions.playerManager.health > damage){
                    projectileFunctions.serverEvents.sendEventFromOther(senderID, "ue", "d", projectileFunctions.serverComm.ID + "~" + damage);
                }
                else{
                    projectileFunctions.serverEvents.sendEventFromOther(senderID, "ue", "death", projectileFunctions.serverComm.ID + "");
                }
            }
        }
        //Collider coll = cols[i];
        //if(!Physics.Raycast(transform.position, projectileFunctions.playerCam.transform.position - transform.position, Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position), stopFrom)){
        //    if(coll.gameObject.layer == 3){
        //        if(projectileFunctions.playerManager.health > damage){
        //            projectileFunctions.serverEvents.sendEvent("ue", "damage", projectileFunctions.serverComm.ID + "~" + damage);
        //        }
        //        else{
        //            projectileFunctions.serverEvents.sendEvent("ue", "death", projectileFunctions.serverComm.ID + "");
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
        projectileFunctions.soundManager.playSound(bounceSound, transform.position, 1f, 2f);
    }
}
