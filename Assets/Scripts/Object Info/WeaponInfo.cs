using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public int projectileID;
    public float bulletTravelSpeed;
    public float damage;
    public float startDistance;

    public int shootSound;
    public float shootPitch;
    public float shootVolume;
    public int reloadSound;
    
    public float cooldown;
    public float equipCooldown;
    public float cooldownTimer;
    public float reloadTime;

    public bool reloadable;

    public int objectsInClip;
    public int clipSize;

    private void Update() {
        cooldownTimer -= Time.deltaTime;
    }
}
