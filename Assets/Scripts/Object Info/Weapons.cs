using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Weapons : MonoBehaviour
{
    public WeaponInfo equippedWeapon;
    int weaponIDForNothing;

    [Header("Debug:")]
    public bool canShoot;
    public bool canADS;
    public int objectsInClip;
    public float cooldownTimer;
    public float charge;

    [Header("References:")]
    public Look look;
    public ProjectileManager projectileManager;
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public GameObject cam;
    public Rigidbody playerRB;
    public List<WeaponInfo> weaponInfos;
    public ControlsManager controlsManager;
    public TextMeshProUGUI objectsInClipText;
	public TextMeshProUGUI equippedWeaponText;
	public Slider chargeIndicator;

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
        weaponIDForNothing = weaponInfos.Count - 1;
        setWeapon(0);
    }
    public void setWeapon(int weaponID){
        foreach(WeaponInfo weapon in weaponInfos){
            weapon.gameObject.SetActive(false);
        }
        if(weaponID == -1){
            setWeapon(weaponIDForNothing);
        }
        else{
            equippedWeapon = weaponInfos[weaponID];
            equippedWeapon.gameObject.SetActive(true);
            objectsInClip = equippedWeapon.clipSize;
        }
        updateGUI();
    }

    public void shoot(float damageMultiplier){
        if(canShoot && cooldownTimer <= 0){
            if(objectsInClip > 0){
                projectileManager.createProjectile(0, equippedWeapon.projectileID, equippedWeapon.damage * damageMultiplier, equippedWeapon.bulletSpawnPos.position, equippedWeapon.bulletSpeed * cam.transform.forward + playerRB.velocity);
                serverEvents.sendEvent("ue", "pr", equippedWeapon.projectileID + "~" + equippedWeapon.damage * damageMultiplier + "~" + equippedWeapon.bulletSpawnPos.position + "~" + (cam.transform.forward * equippedWeapon.bulletSpeed + playerRB.velocity) + "~" + equippedWeapon.shootSound + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
                objectsInClip -= 1;
                updateGUI();
                cooldownTimer = equippedWeapon.cooldown;
            }
            else if(equippedWeapon.autoReload){
                reload();
            }
        }
    }

    public void reload(){
        if(cooldownTimer <= 0 && objectsInClip < equippedWeapon.clipSize){
            objectsInClipText.text = "--/" + equippedWeapon.clipSize;
            objectsInClip = equippedWeapon.clipSize;
            cooldownTimer = equippedWeapon.reloadTime;
            Invoke("updateGUI", equippedWeapon.reloadTime);
        }
    }   

    private void Update() {
        cooldownTimer -= Time.deltaTime;
        if(controlsManager.weaponUse){
            if(equippedWeapon.charge){
                charge = Mathf.Clamp(charge += Time.deltaTime, 0, equippedWeapon.maxCharge);
                updateGUI();
            }
            else{
                shoot(1f);
            }
        }
        else{
            if(charge > 0){
                shoot(charge/equippedWeapon.maxCharge);
                charge = 0;
                updateGUI();
            }
        }

        if(controlsManager.reloading){
            reload();
        }
        look.FOV = Mathf.Lerp(look.FOV, regularFOV, FOVChangeSpeed);
        look.gunCamFOV = Mathf.Lerp(look.gunCamFOV, gunCamRegularFOV, FOVChangeSpeed);
    }

    public void resetAll(){
        objectsInClip = equippedWeapon.clipSize;
        cooldownTimer = 0;
        updateGUI();
    }

    public void updateGUI(){
        objectsInClipText.text = objectsInClip + "/" + equippedWeapon.clipSize;
        equippedWeaponText.text = equippedWeapon.name;
        if(equippedWeapon.charge){
            chargeIndicator.gameObject.SetActive(true);
            chargeIndicator.value = charge/equippedWeapon.maxCharge;
        }
        else{
            chargeIndicator.gameObject.SetActive(false);
        }
    }
}
