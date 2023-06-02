using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    public float timeToExplode;
    float damage;
    public float radius;
    int senderID;
    public Rigidbody rb;
    public Collider coll;

    ProjectileFunctions projectileFunctions;

    public int bangSound;

    public GameObject explosionParticles;
    public LayerMask stopFrom;
    public float camShakeAmplitude;
    public float minCamShake;
    public float minHeight;

    public void setInfo(Vector3 givenVelocity, float givenDamage, int givenSenderID, ProjectileFunctions givenProjectileFunctions){
        projectileFunctions = givenProjectileFunctions;
        senderID = givenSenderID;
        damage = givenDamage;
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
    }

    private void Start() {
        Physics.IgnoreCollision(coll, projectileFunctions.playerColl, true);
    }

    private void FixedUpdate() {
        if(transform.position.y <= minHeight){
            rb.velocity *= -1;
        }
    }

    void explode(){
        Destroy(Instantiate(explosionParticles, transform.position, Quaternion.identity), 5f);
        projectileFunctions.soundManager.playSound(bangSound, transform.position, 1f, 1f);
        projectileFunctions.Explosion(transform.position, radius, damage, 0, true, stopFrom, senderID);
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll){
        print(coll.gameObject.name);
        explode();
    }
}
