using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float currentTravelSpeed = 0f;
    ServerEvents serverEvents;
    float damage;
    public float lifeTime;
    bool doesDamage = false;

    public void goTo(float travelSpeed, ServerEvents givenServerEvents, float givenDamage, bool givenDoesDamage){
        currentTravelSpeed = travelSpeed;
        serverEvents = givenServerEvents;
        damage = givenDamage;
        doesDamage = givenDoesDamage;
    }

    void Start(){
        Invoke("Destroy", lifeTime);
    }

    void Destroy(){
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * currentTravelSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider coll) {
        if(coll.gameObject.layer == 7 && doesDamage){
            Debug.Log("Hit game object: " + coll.gameObject.name);
            serverEvents.sendEvent("universalEvent", "damage", coll.gameObject.name + "~" + damage);
        }
        Destroy(this.gameObject);
    }
}
