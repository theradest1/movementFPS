using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Molley : MonoBehaviour
{
    public Rigidbody rb;
    public int bangSound;
    ProjectileFunctions projectileFunctions;
    public Collider coll;
    public float minHeight;
    public float explodeTime;
    public GameObject firePrefab;
    public int userID;
    public float damage;
    public float fireLifetime;

    public void setInfo(Vector3 givenVelocity, ProjectileFunctions givenProjectileFunctions){
        rb.velocity = givenVelocity;
        projectileFunctions = givenProjectileFunctions;
        userID = projectileFunctions.serverComm.ID;
    }

    private void Start() {
        Physics.IgnoreCollision(coll, projectileFunctions.playerColl, true);
        Invoke("explode", explodeTime);
    }

    void explode(){
        projectileFunctions.soundManager.playSound(bangSound, transform.position, 1f, 1f);
        
        HurtArea fire = Instantiate(firePrefab, transform.position, Quaternion.identity).GetComponent<HurtArea>();
        fire.userID = userID;
        fire.damage = damage;
        fire.slowStop(fireLifetime);
        Debug.Log("bang (molley)");
        Destroy(this.gameObject);
    }

    private void FixedUpdate() {
        if(transform.position.y <= minHeight){
            rb.velocity *= -1;
        }
    }

    void OnCollisionEnter(Collision coll){
        explode();
    }
}
