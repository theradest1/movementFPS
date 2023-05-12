using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInfo : MonoBehaviour
{
    [Header("Server controled variables:")]
    public float weight;
    public float bulletSpeed;
    public float damage;
    public float cooldown;
    public float reloadTime;
    public int clipSize;
    public float headShotMult;
    public float recoilVerticalMin;
    public float recoilVerticalMax;
    public float recoilHorizontal;
    public float objectSpeedADSMult = 1f;



    [Header("Sound system settings:")]
    public float shootPitch = 1f;
    public float shootVolume = 1f;
    public int shootSound;
    public int startReloadSoundID;
    public int reloadPerBulletSoundID;
    public int endReloadSoundID;

    [Header("Gun settings:")]
    public int projectileID;
    public bool automatic = true;
    public bool canADS = false;
    public bool autoReload; //when trying to shoot without any bullets left, it will reload automatically (might not want this for things like a rocket launcher)
    public bool charge = false;
    public float maxCharge;
    public bool incrimentalReload = false;

    public Transform restingTransform;
    public Transform scopingTransform;
    public Transform bulletSpawnPos;



    public void setVars(string[] vars){
        weight = float.Parse(vars[1]);
        bulletSpeed = float.Parse(vars[2]);
        damage = float.Parse(vars[3]);
        cooldown = float.Parse(vars[4]);
        reloadTime = float.Parse(vars[6]);
        clipSize = int.Parse(vars[7]);
        headShotMult = float.Parse(vars[8]);
        recoilVerticalMin = float.Parse(vars[9]);
        recoilVerticalMax = float.Parse(vars[10]);
        recoilHorizontal = float.Parse(vars[11]);
    }
}
