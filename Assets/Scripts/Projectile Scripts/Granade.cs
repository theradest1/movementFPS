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
    public Collider coll;

    public void setInfo(Vector3 givenVelocity, float givenDamage, int givenSenderID, ProjectileFunctions givenProjectileFunctions){
        projectileFunctions = givenProjectileFunctions;
        senderID = givenSenderID;
        damage = givenDamage;
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
        distanceVisualizer.transform.localScale = new Vector3(radius * 2 / transform.localScale.x, .1f, radius * 2 / transform.localScale.x);
    }

    private void Start() {
        Physics.IgnoreCollision(coll, projectileFunctions.playerColl, true);
    }

    private void Update() {
        distanceVisualizer.transform.eulerAngles = new Vector3(0f, 0f, 0f);
    }

    void explode(){
        Destroy(Instantiate(explosionParticles, transform.position, Quaternion.identity), 5f);
        projectileFunctions.soundManager.playSound(bangSound, transform.position, 1f, 1f);
        projectileFunctions.Explosion(transform.position, radius, damage, 0, true, stopFrom, senderID);
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
