using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    ServerEvents serverEvents;
    float damage;
    int senderID;
    bool sendDamageEvent;

    GameObject fakeBullet;
    GameObject fakeBulletPrefab;
    GameObject bulletHolePrefab;
    GameObject bulletHole;
    public Rigidbody rb;
    Vector3 velocityAfterDestroy;

    public float maxLifeTime;
    public float fakeBulletAccuracy;
    public float bulletHoleLife;
    public float fakeBulletLifeTime;
    bool destroyed = false;

    
    public void setInfo(int givenSenderID, float givenDamage, ServerEvents givenServerEvents){
        Invoke("destroy", maxLifeTime);
        serverEvents = givenServerEvents;
        senderID = givenSenderID;
        damage = givenDamage;
        fakeBullet = Instantiate(fakeBulletPrefab, transform.position, Quaternion.identity);
    }

    void Update()
    {
        if(destroyed){
            transform.position += velocityAfterDestroy * Time.deltaTime;
        }
        if(fakeBullet != null){
            fakeBullet.transform.position = Vector3.Lerp(fakeBullet.transform.position, transform.position, fakeBulletAccuracy * Time.deltaTime);
        }

        if(destroyed){
            Destroy(fakeBullet, fakeBulletLifeTime);
            //this.gameObject.SetActive(false);
            Destroy(bulletHole, bulletHoleLife);
            Destroy(this.gameObject, fakeBulletLifeTime + .1f);
        }
    }

    void OnCollisionEnter(Collision coll) {
        if(coll.gameObject.layer == 7 && sendDamageEvent){
            if(serverEvents.clientScripts[serverEvents.clientIDs.IndexOf(int.Parse(coll.gameObject.name))].health <= damage){
                serverEvents.sendEvent("ue", "death", coll.gameObject.name);
            }
            else{
                serverEvents.sendEvent("ue", "damage", coll.gameObject.name + "~" + damage);
            }
        }
        else{
            bulletHole = Instantiate(bulletHolePrefab, coll.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, coll.contacts[0].normal));
            bulletHole.transform.RotateAround(bulletHole.transform.position, coll.contacts[0].normal, Random.Range(0f, 360f));
        }
        velocityAfterDestroy = rb.velocity;
        Destroy(rb);
        destroyed = true;
    }
}
