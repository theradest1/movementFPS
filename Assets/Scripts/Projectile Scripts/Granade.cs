using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granade : MonoBehaviour
{
    public float timeToExplode;
    float damage;
    public void setInfo(float givenDamage){
        damage = givenDamage;
    }
}
