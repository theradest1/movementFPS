using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    public Rigidbody rb;
    public float timeToExplode;
    public void setInfo(Vector3 givenVelocity){
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
    }

    void explode(){
        Debug.Log("bang (granade)");
    }

    void Start(){
        Debug.Log("Flash");
    }
}
