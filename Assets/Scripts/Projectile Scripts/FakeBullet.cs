using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeBullet : MonoBehaviour
{
    int senderID;

    public GameObject bulletHolePrefab;
    public float bulletHoleLife;
    public float maxLifeTime;
    public int playerLayer;
    public int otherPlayerLayer;

    public void setInfo(int givenSenderID){
        senderID = givenSenderID;
        Destroy(this.gameObject, maxLifeTime);
    }

    void OnCollisionEnter(Collision coll) {
        if(coll.gameObject.layer != playerLayer && coll.gameObject.layer != otherPlayerLayer){
            GameObject bulletHole = Instantiate(bulletHolePrefab, coll.contacts[0].point, Quaternion.FromToRotation(Vector3.forward, coll.contacts[0].normal));
            bulletHole.transform.RotateAround(bulletHole.transform.position, coll.contacts[0].normal, Random.Range(0f, 360f));
            Destroy(bulletHole, bulletHoleLife);
        }
        Destroy(this.gameObject);
    }
}
