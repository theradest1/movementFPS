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
    public float charge;
    public float cooldownTimer;
    public float reloadTimer;
    public bool reloading;
    public bool releasedShoot = true;

    [Header("References:")]
    public Look look;
    public ProjectileManager projectileManager;
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public GameObject cam;
    public Camera camComponent;
    public Camera gunCamComponent;
    public Rigidbody playerRB;
    public List<WeaponInfo> weaponInfos;
    public ControlsManager controlsManager;
    public TextMeshProUGUI objectsInClipText;
	public TextMeshProUGUI equippedWeaponText;
	public Slider chargeIndicator;
    public GameObject weaponContainer;

    [Header("Settings:")]
    public float scopingFOV;
    public float gunCamScopingFOV;
    public float regularFOV;
    public float gunCamRegularFOV;
    public float FOVChangeSpeed;
    public float gunCamFOVChangeSpeed;
    
    public float aimSpeed;
    public float relaxSpeed;

    public float weaponContainerSpeed;
    public float weaponDistanceMult;
    public float weaponDistanceMultADS;


    private void Start() {
        weaponIDForNothing = weaponInfos.Count - 1;
        setWeapon(0);
    }
    public void setWeapon(int weaponID){
        resetAll();
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
        if(releasedShoot || equippedWeapon.automatic){
            releasedShoot = false;
            //projectileManager.createProjectile(0, equippedWeapon.projectileID, equippedWeapon.damage * damageMultiplier, equippedWeapon.bulletSpawnPos.position, equippedWeapon.bulletSpeed * cam.transform.forward + playerRB.velocity);
            projectileManager.createProjectile(0, equippedWeapon.projectileID, equippedWeapon.damage * damageMultiplier, cam.transform.position, equippedWeapon.bulletSpawnPos.rotation, equippedWeapon.bulletSpeed * cam.transform.forward + playerRB.velocity, equippedWeapon.bulletSpawnPos.position);
            //soundManager.playSound(equippedWeapon.shootSound, cam.transform.position, equippedWeapon.shootVolume, equippedWeapon.shootPitch, cam.transform);
            
            objectsInClip -= 1;
            updateGUI();
            cooldownTimer = equippedWeapon.cooldown;
        }
    }

    public void reload(){
        reloading = true;

        if(equippedWeapon.incrimentalReload){
            reloadTimer = equippedWeapon.reloadTime/equippedWeapon.clipSize;
        }
        else{
            reloadTimer = equippedWeapon.reloadTime;
            objectsInClipText.text = "--/" + equippedWeapon.clipSize;
        }
    }

    private void Update() {
        if(!controlsManager.weaponUse){
            releasedShoot = true;
        }

        cooldownTimer -= Time.deltaTime;
        reloadTimer -= Time.deltaTime;
        if(controlsManager.weaponUse && objectsInClip > 0 && cooldownTimer <= 0 && canShoot){
            reloading = false;
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
            else if(objectsInClip <= 0 && equippedWeapon.autoReload && !reloading && cooldownTimer <= 0){
                reload();
                Debug.Log("Auto reload");
            }
        }

        if(controlsManager.reloading && !reloading && objectsInClip <= equippedWeapon.clipSize && cooldownTimer <= 0){
            reload();
        }
        else if(reloading && reloadTimer <= 0){
            if(equippedWeapon.incrimentalReload && objectsInClip < equippedWeapon.clipSize){
                objectsInClip++;
                reloadTimer = equippedWeapon.reloadTime/equippedWeapon.clipSize;
            }
            else{
                reloading = false;
                objectsInClip = equippedWeapon.clipSize;
            }
            updateGUI();
        }


        //ADSing stuffs:
        if(equippedWeapon.canADS && controlsManager.aiming){
            gunCamComponent.fieldOfView = Mathf.Lerp(gunCamComponent.fieldOfView, gunCamScopingFOV, FOVChangeSpeed * Time.deltaTime);
            camComponent.fieldOfView = Mathf.Lerp(camComponent.fieldOfView, scopingFOV, FOVChangeSpeed * Time.deltaTime);
            
            equippedWeapon.transform.position = Vector3.Lerp(equippedWeapon.transform.position, equippedWeapon.scopingTransform.position, aimSpeed * Time.deltaTime);
            //equippedWeapon.transform.rotation = Quaternion.Slerp(equippedWeapon.transform.rotation, equippedWeapon.scopingTransform.rotation, aimSpeed * Time.deltaTime);
            
            weaponContainer.transform.position -= playerRB.velocity * weaponDistanceMultADS * Time.deltaTime;
            
        }
        else{
            gunCamComponent.fieldOfView = Mathf.Lerp(gunCamComponent.fieldOfView, gunCamRegularFOV, FOVChangeSpeed * Time.deltaTime);
            camComponent.fieldOfView = Mathf.Lerp(camComponent.fieldOfView, regularFOV, FOVChangeSpeed * Time.deltaTime);
            
            equippedWeapon.transform.position = Vector3.Lerp(equippedWeapon.transform.position, equippedWeapon.restingTransform.position, relaxSpeed * Time.deltaTime);
            
            weaponContainer.transform.position -= playerRB.velocity * weaponDistanceMult * Time.deltaTime;
        }

        weaponContainer.transform.localPosition = Vector3.Lerp(weaponContainer.transform.localPosition, Vector3.zero, weaponContainerSpeed * Time.deltaTime);
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
