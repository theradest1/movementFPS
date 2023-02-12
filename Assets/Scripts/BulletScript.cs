using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float currentTravelSpeed = 0f;
    ServerEvents serverEvents;
    //Vector3 pastPos;
    float damage;
    public float lifeTime;
    bool doesDamage = false;
    public Rigidbody rb;
    GameObject fakeBullet;
    public float fakeBulletAccuracy;

    public void goTo(float travelSpeed, ServerEvents givenServerEvents, float givenDamage, bool givenDoesDamage, GameObject giveFakeBullet){
        //currentTravelSpeed = travelSpeed;
        rb.velocity = transform.forward * travelSpeed;
        serverEvents = givenServerEvents;
        damage = givenDamage;
        doesDamage = givenDoesDamage;
        fakeBullet = giveFakeBullet;
    }

    void Start(){
        Invoke("destroy", lifeTime);
    }

    void destroy(){
        Destroy(fakeBullet);
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        fakeBullet.transform.position = Vector3.Lerp(fakeBullet.transform.position, transform.position, fakeBulletAccuracy * Time.deltaTime);
    }

    void OnTriggerEnter(Collider coll) {
        if(coll.gameObject.layer == 7 && doesDamage){
            Debug.Log("Hit game object: " + coll.gameObject.name);
            serverEvents.sendEvent("universalEvent", "damage", coll.gameObject.name + "~" + damage);
        }
        destroy();
    }
}
