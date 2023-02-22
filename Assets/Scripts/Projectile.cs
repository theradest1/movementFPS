using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    int projectileID;
    int senderID;
    float damage;
    public void setInfo(int givenSenderID, float givenDamage, int givenProjectileID){
        damage = givenDamage;
        senderID = givenSenderID;
        projectileID = givenProjectileID;
    }

    
}
