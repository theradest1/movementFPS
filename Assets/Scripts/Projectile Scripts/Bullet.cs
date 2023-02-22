using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    ServerEvents serverEvents;
    float damage;

    GameObject fakeBullet;
    public GameObject fakeBulletPrefab;
    public GameObject bulletHolePrefab;
    GameObject bulletHole;
    public Rigidbody rb;
    Vector3 velocityAfterDestroy;

    public float maxLifeTime;
    public float fakeBulletAccuracy;
    public float bulletHoleLife;
    public float fakeBulletLifeTime;
    bool destroyed = false;
    
    public void setInfo(Vector3 givenVelocity, float givenDamage, ServerEvents givenServerEvents, Vector3 fouxBulletPos){
        rb.velocity = givenVelocity;
        serverEvents = givenServerEvents;
        damage = givenDamage;
        fakeBullet = Instantiate(fakeBulletPrefab, fouxBulletPos, Quaternion.identity);

        Destroy(fakeBullet, maxLifeTime);
        Destroy(this.gameObject, maxLifeTime);
    }

    void Update()
    {
        if(fakeBullet != null){
            if(!destroyed){
                fakeBullet.transform.position += rb.velocity/2 * Time.deltaTime;
            }
            fakeBullet.transform.position = Vector3.Lerp(fakeBullet.transform.position, transform.position, fakeBulletAccuracy * Time.deltaTime);
        }

        if(destroyed){
            transform.position += velocityAfterDestroy * Time.deltaTime;
        }
        else{
            velocityAfterDestroy = rb.velocity;
        }

    }

    void OnCollisionEnter(Collision coll) {
        if(coll.gameObject.layer != 3 && !destroyed){
            if(coll.gameObject.layer == 7){
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
            Destroy(rb);

            destroyed = true;

            Destroy(fakeBullet, fakeBulletLifeTime);
            Destroy(bulletHole, bulletHoleLife);
            Destroy(this.gameObject, fakeBulletLifeTime + .1f);
        }
    }
}
