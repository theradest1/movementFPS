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
    public bool scoping;
    public bool toggleScoping;
    public bool releasedAiming;

    [Header("References:")]
    public Look look;
    public ProjectileManager projectileManager;
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public SoundManager soundManager;
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
    public float weaponContainerRotateSpeed;
    public float weaponDistanceMult;
    public float weaponDistanceMultADS;

    public float spreadADSMult;
    public float generalRecoilMult;
    public float horizontalRecoilMult;
    public float verticalRecoilMult;
    public float camRecoilPercent;

    public float getCooldownPercentage(){
        if(reloading && objectsInClip < equippedWeapon.clipSize){
            if(equippedWeapon.incrimentalReload){
                return reloadTimer/(equippedWeapon.reloadTime/equippedWeapon.clipSize);
            }
            return reloadTimer/equippedWeapon.reloadTime;
        }
        return cooldownTimer/equippedWeapon.cooldown;
    }

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
            Vector3 bulletSpeed;
            if(scoping){
                bulletSpeed = equippedWeapon.ADSBulletSpeed * cam.transform.forward + playerRB.velocity;
            }
            else{
                bulletSpeed = equippedWeapon.bulletSpeed * cam.transform.forward + playerRB.velocity;
            }

            projectileManager.createProjectile(serverComm.ID, equippedWeapon.projectileID, equippedWeapon.damage * damageMultiplier, cam.transform.position, equippedWeapon.bulletSpawnPos.rotation, bulletSpeed, equippedWeapon.bulletSpawnPos.position);
            soundManager.playSound(equippedWeapon.shootSound, cam.transform.position, equippedWeapon.shootVolume, equippedWeapon.shootPitch, cam.transform);

            objectsInClip -= 1;
            updateGUI();
            cooldownTimer = equippedWeapon.cooldown;

            //recoil
            //weaponContainer.transform.localEulerAngles = equippedWeapon.transform.localEulerAngles - new Vector3(Random.Range(equippedWeapon.recoilVerticalMin * spreadADSMult * generalRecoilMult, equippedWeapon.recoilVerticalMax * spreadADSMult * generalRecoilMult), 0f, 0f);
			if(scoping){
                weaponContainer.transform.Rotate(0f, Random.Range(-equippedWeapon.recoilHorizontal * spreadADSMult * generalRecoilMult, equippedWeapon.recoilHorizontal * spreadADSMult * generalRecoilMult), 0f);
                look.camRotX -= Random.Range(equippedWeapon.recoilVerticalMin * spreadADSMult * generalRecoilMult, equippedWeapon.recoilVerticalMax * spreadADSMult * generalRecoilMult) * camRecoilPercent;
            }
            else{
                weaponContainer.transform.Rotate(0f, Random.Range(-equippedWeapon.recoilHorizontal, equippedWeapon.recoilHorizontal) * generalRecoilMult * horizontalRecoilMult, 0f);
                weaponContainer.transform.Rotate(Random.Range(0, -equippedWeapon.recoilVerticalMax) * generalRecoilMult * verticalRecoilMult, 0f, 0f);
                
                look.camRotX -= Random.Range(equippedWeapon.recoilVerticalMin * generalRecoilMult, equippedWeapon.recoilVerticalMax * generalRecoilMult) * camRecoilPercent * verticalRecoilMult;
                //playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(0, Random.Range(-equippedWeapon.recoilHorizontal, equippedWeapon.recoilHorizontal) * generalRecoilMult * camRecoilPercent * horizontalRecoilMult, 0));
                look.rotY -= Random.Range(-equippedWeapon.recoilHorizontal, equippedWeapon.recoilHorizontal) * generalRecoilMult * camRecoilPercent * horizontalRecoilMult;
            }
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
        if(!controlsManager.aiming){
            releasedAiming = true;
        }
        if(toggleScoping && equippedWeapon.canADS){
            if(releasedAiming && controlsManager.aiming){
                scoping = !scoping;
                releasedAiming = false;
            }
        }
        else if(equippedWeapon.canADS){
            scoping = controlsManager.aiming;
        }
        else{
            scoping = false;
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
        if(scoping){
            gunCamComponent.fieldOfView = Mathf.Lerp(gunCamComponent.fieldOfView, gunCamScopingFOV, FOVChangeSpeed * Time.deltaTime);
            camComponent.fieldOfView = Mathf.Lerp(camComponent.fieldOfView, scopingFOV, FOVChangeSpeed * Time.deltaTime);
            
            equippedWeapon.transform.position = Vector3.Lerp(equippedWeapon.transform.position, equippedWeapon.scopingTransform.position, aimSpeed * Time.deltaTime);
            //equippedWeapon.transform.localRotation = Quaternion.Slerp(equippedWeapon.transform.localRotation, equippedWeapon.scopingTransform.localRotation, aimSpeed * Time.deltaTime);
            
            weaponContainer.transform.position -= playerRB.velocity * weaponDistanceMultADS * Time.deltaTime;
            
        }
        else{
            gunCamComponent.fieldOfView = Mathf.Lerp(gunCamComponent.fieldOfView, gunCamRegularFOV, FOVChangeSpeed * Time.deltaTime);
            camComponent.fieldOfView = Mathf.Lerp(camComponent.fieldOfView, regularFOV, FOVChangeSpeed * Time.deltaTime);
            
            equippedWeapon.transform.position = Vector3.Lerp(equippedWeapon.transform.position, equippedWeapon.restingTransform.position, relaxSpeed * Time.deltaTime);
            
            weaponContainer.transform.position -= playerRB.velocity * weaponDistanceMult * Time.deltaTime;
        }

        weaponContainer.transform.localRotation = Quaternion.Slerp(weaponContainer.transform.localRotation, Quaternion.identity, weaponContainerRotateSpeed * Time.deltaTime);
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
