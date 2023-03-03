using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassInfo : MonoBehaviour
{
    public float health = 500;
    public float damageMult = 1;
    public float ammoCapacityMult = 1;
    public float toolCapacityMult = 1;
    public float speedMult = 1;
    public float reloadSpeedMult = 1;
    public float gunFireSpeedMult = 1;
    public float healAmount = .01f;
    public float healCooldown = 6f;

    public void setVars(string[] vars){
        health = float.Parse(vars[1]);
        damageMult = float.Parse(vars[2]);
        ammoCapacityMult = float.Parse(vars[3]);
        toolCapacityMult = float.Parse(vars[4]);
        speedMult = float.Parse(vars[5]);
        reloadSpeedMult = float.Parse(vars[6]);
        gunFireSpeedMult = float.Parse(vars[7]);
        healAmount = float.Parse(vars[8]);
        healCooldown = float.Parse(vars[9]);
    }

}
