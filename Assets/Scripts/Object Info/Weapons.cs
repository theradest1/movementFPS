using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public WeaponInfo equippedWeapon;

    [Header("Debug:")]
    public bool canShoot;
    public bool canADS;
    public int objectsInClip;
    public float cooldownTimer;

    [Header("References:")]
    public Look look;
    public ProjectileManager projectileManager;
    public ServerComm serverComm;
    public GameObject cam;
    public Rigidbody playerRB;
    public List<WeaponInfo> weaponInfos;
    public ControlsManager controlsManager;

    [Header("Settings:")]
    public float scopingFOV;
    public float gunCamScopingFOV;
    public float regularFOV;
    public float gunCamRegularFOV;
    public float FOVChangeSpeed;
    public float gunCamFOVChangeSpeed;
    public float aimSpeed;
    public float relaxSpeed;

    private void Start() {
        setWeapon(0);
    }
    public void setWeapon(int weaponID){
        Debug.Log(weaponID);
        foreach(WeaponInfo weapon in weaponInfos){
            weapon.gameObject.SetActive(false);
        }
        equippedWeapon = weaponInfos[weaponID];
        equippedWeapon.gameObject.SetActive(true);
        objectsInClip = equippedWeapon.clipSize;
    }

    public void shoot(){
        Debug.Log("Shoot");
        if(canShoot && cooldownTimer <= 0){
            if(objectsInClip > 0){
                projectileManager.createProjectile(serverComm.ID, equippedWeapon.projectileID, equippedWeapon.damage, equippedWeapon.bulletSpawnPos.position, equippedWeapon.bulletSpeed * cam.transform.forward + playerRB.velocity);
                objectsInClip -= 1;
                cooldownTimer = equippedWeapon.cooldown;
            }
            else if(equippedWeapon.autoReload){
                reload();
            }
        }
    }

    public void reload(){
        if(cooldownTimer <= 0){
            Debug.Log("Reloaded");
            objectsInClip = equippedWeapon.clipSize;
            cooldownTimer = equippedWeapon.reloadTime;
        }
    }   

    private void Update() {
        cooldownTimer -= Time.deltaTime;
        if(controlsManager.weaponUse){
            shoot();
        }
        look.FOV = Mathf.Lerp(look.FOV, regularFOV, FOVChangeSpeed);
        look.gunCamFOV = Mathf.Lerp(look.gunCamFOV, gunCamRegularFOV, FOVChangeSpeed);
    }

    public void resetAll(){
        objectsInClip = equippedWeapon.clipSize;
        cooldownTimer = 0;
    }
}
