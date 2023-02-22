using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float timeToExplode;
    public Rigidbody rb;

    public void setInfo(Vector3 givenVelocity){
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
    }

    void explode(){
        Debug.Log("bang (smoke)");
        
    }
}
