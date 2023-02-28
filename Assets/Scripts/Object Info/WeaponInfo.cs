using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    public float speedMultiplier = 1f;
    public int projectileID;
    public float bulletTravelSpeed;
    public float damage;
    public float startDistance;

    public int shootSound;
    public float shootPitch = 1f;
    public float shootVolume = 1f;
    public int reloadSound;
    
    public float cooldown;
    public float equipCooldown;
    [HideInInspector]
    public float cooldownTimer;
    public float reloadTime;

    public bool reloadable;
    public bool automatic = true;

    public int objectsInClip;
    public int clipSize;

    private void Update() {
        cooldownTimer -= Time.deltaTime;
    }
}
