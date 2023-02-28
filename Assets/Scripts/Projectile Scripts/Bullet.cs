using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
    public Collider coll;
    bool destroyed = false;
    ProjectileFunctions projectileFunctions;
    
    public void setInfo(Vector3 givenVelocity, float givenDamage, Vector3 fouxBulletPos, ProjectileFunctions givenProjectileFunctions){
        rb.velocity = givenVelocity;
        projectileFunctions = givenProjectileFunctions;
        damage = givenDamage;
        fakeBullet = Instantiate(fakeBulletPrefab, fouxBulletPos, Quaternion.identity);
        fakeBullet.transform.LookAt(fakeBullet.transform.position + rb.velocity);

        Destroy(fakeBullet, maxLifeTime);
        Destroy(this.gameObject, maxLifeTime);
    }

    private void Start() {
        Physics.IgnoreCollision(coll, projectileFunctions.playerColl, true);
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
                if(projectileFunctions.serverEvents.clientScripts[projectileFunctions.serverEvents.clientIDs.IndexOf(int.Parse(coll.gameObject.name))].health <= damage){
                    projectileFunctions.serverEvents.sendEvent("ue", "death", coll.gameObject.name);
                }
                else{
                    projectileFunctions.serverEvents.sendEvent("ue", "damage", coll.gameObject.name + "~" + damage);
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
