using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float timeToExplode;
    public Rigidbody rb;
    public GameObject smokePrefab;
    public float smokeTime;
    public int bangSound;
    ProjectileFunctions projectileFunctions;

    public void setInfo(Vector3 givenVelocity, ProjectileFunctions givenProjectileFunctions){
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
        projectileFunctions = givenProjectileFunctions;
    }

    void explode(){
        projectileFunctions.soundManager.playSound(bangSound, transform.position, 1f, 1f);
        Destroy(Instantiate(smokePrefab, transform.position + Vector3.up, Quaternion.identity), smokeTime);
        Destroy(this.gameObject);
    }
}
