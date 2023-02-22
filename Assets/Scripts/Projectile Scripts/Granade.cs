using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float timeToExplode;
    float damage;
    public Rigidbody rb;

    public void setInfo(Vector3 givenVelocity, float givenDamage){
        damage = givenDamage;
        rb.velocity = givenVelocity;
        Invoke("explode", timeToExplode);
    }

    void explode(){
        Debug.Log("bang (granade)");
    }
}
