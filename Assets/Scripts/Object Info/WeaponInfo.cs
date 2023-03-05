using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    [HideInInspector]
    public float speedMultiplier = 1f;
    [HideInInspector]
    public float bulletTravelSpeed;
    [HideInInspector]
    public float damage;
    [HideInInspector]
    public float cooldown;
    [HideInInspector]
    public float equipCooldown;
    [HideInInspector]
    public float reloadTime;
    [HideInInspector]
    public int clipSize;
    [HideInInspector]
    public int objectsInClip;
    [HideInInspector]
    public float headShotMult;

    public int projectileID;
    public float startDistance;

    public int shootSound;
    public float shootPitch = 1f;
    public float shootVolume = 1f;
    public int reloadSound;
    
    [HideInInspector]
    public float cooldownTimer;

    public bool reloadable;
    public bool automatic = true;


    public bool gun = true;
    
    public bool tool = false;
    public bool main = true;
    public bool secondary = false;

    private void Update() {
        cooldownTimer -= Time.deltaTime;
    }

    public void setVars(string[] vars){
        speedMultiplier = float.Parse(vars[1]);
        bulletTravelSpeed = float.Parse(vars[2]);
        damage = float.Parse(vars[3]);
        cooldown = float.Parse(vars[4]);
        equipCooldown = float.Parse(vars[5]);
        reloadTime = float.Parse(vars[6]);
        clipSize = int.Parse(vars[7]);
        objectsInClip = int.Parse(vars[7]);
        headShotMult = float.Parse(vars[8]);
    }
}
