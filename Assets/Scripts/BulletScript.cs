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
    public float minFinalDistance;
    bool destroyed = false;
    public float bulletHoleLife;
    GameObject bulletHole;
    public GameObject bulletHolePrefab;
    float travelSpeed;
    Vector3 travelDirection;
    public float timeBeforeDestroyFake;
    GameObject flash;
    public float flashTime;

    public void goTo(float giveTravelSpeed, ServerEvents givenServerEvents, float givenDamage, bool givenDoesDamage, GameObject giveFakeBullet){
        //currentTravelSpeed = travelSpeed;
        travelSpeed = giveTravelSpeed;
        rb.velocity = transform.forward * giveTravelSpeed;
        serverEvents = givenServerEvents;
        damage = givenDamage;
        doesDamage = givenDoesDamage;
        fakeBullet = giveFakeBullet;
        travelDirection = transform.forward;

    }

    void Start(){
        Invoke("destroy", lifeTime);
        flash = transform.GetChild(0).gameObject;
        Destroy(flash, flashTime);
    }

    void destroy(){
        Destroy(rb);
        destroyed = true;
    }

    // Update is called once per frame
    void Update()
    {

        if(destroyed){
            this.transform.position += travelDirection * travelSpeed * Time.deltaTime;
        }
        if(fakeBullet != null){
            fakeBullet.transform.position = Vector3.Lerp(fakeBullet.transform.position, transform.position, fakeBulletAccuracy * Time.deltaTime);
        }

        if(destroyed){
            Destroy(fakeBullet, timeBeforeDestroyFake);
            //this.gameObject.SetActive(false);
            Destroy(bulletHole, bulletHoleLife);
            Destroy(this.gameObject, bulletHoleLife + .1f);
        }
    }

    void OnCollisionEnter(Collision coll) {
        if(coll.gameObject.layer == 7 && doesDamage){
            //Debug.Log("Hit game object: " + coll.gameObject.name);
            serverEvents.sendEvent("universalEvent", "damage", coll.gameObject.name + "~" + damage);
        }
        else{
            bulletHole = Instantiate(bulletHolePrefab, coll.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, coll.contacts[0].normal));
            bulletHole.transform.RotateAround(bulletHole.transform.position, coll.contacts[0].normal, Random.Range(0f, 360f));
        }
        destroy();
    }
}
