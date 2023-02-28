using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class WeaponManager : MonoBehaviour
{
    ControlsManager controlsManager;
    movement movementScript;
    public List<WeaponInfo> weapons;
    
    [HideInInspector]
    public WeaponInfo equippedWeapon;

    GameObject cam;
    GameObject player;
    ServerEvents serverEvents;
    GameObject weaponContainer;
    ProjectileManager projectileManager;
    SoundManager soundManager;
    TextMeshProUGUI objectsInClipText;

    //public LayerMask playerMask;
    //public LayerMask aimableMask;

    public float weaponRotationSpeed;
    public float weaponTravelSpeed;
    public float reloadingTimer;
    public bool reloading;
    public bool ableToShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GameObject.Find("Player").GetComponent<movement>();
        projectileManager = GameObject.Find("Player").GetComponent<ProjectileManager>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        cam = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        weaponContainer = GameObject.Find("weapons");
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        objectsInClipText = GameObject.Find("ammo left").GetComponent<TextMeshProUGUI>();

        changeWeapon(1);
    }

    public void resetAllWeapons(){
        for(int weaponID = 0; weaponID < weapons.Count; weaponID++){
            weapons[weaponID].objectsInClip = weapons[weaponID].clipSize;
            weapons[weaponID].cooldownTimer = 0f;
        }
        objectsInClipText.text = equippedWeapon.objectsInClip + "/" + equippedWeapon.clipSize;
    }

    public void changeWeapon(int newWeapon){
        if(newWeapon <= weapons.Count){
            if(equippedWeapon != null){
                equippedWeapon.gameObject.SetActive(false);
            }
            reloading = false;
            ableToShoot = true;
            
            equippedWeapon = weapons[newWeapon - 1];
            equippedWeapon.gameObject.SetActive(true);
            equippedWeapon.transform.localRotation = Quaternion.Euler(50f, equippedWeapon.transform.localEulerAngles.y, equippedWeapon.transform.localEulerAngles.z); 
            
            if(equippedWeapon.cooldownTimer < equippedWeapon.equipCooldown){
                equippedWeapon.cooldownTimer = equippedWeapon.equipCooldown;
            }

            movementScript.speedMultiplierFromWeapon = equippedWeapon.speedMultiplier;
            
            Debug.Log("Changed to " + equippedWeapon.gameObject.name);
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + equippedWeapon.clipSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!controlsManager.shooting){
            ableToShoot = true;
        }
        
        weaponContainer.transform.rotation = Quaternion.Slerp(weaponContainer.transform.rotation, player.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        equippedWeapon.transform.rotation = Quaternion.Slerp(equippedWeapon.transform.rotation, cam.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        if((controlsManager.reloading && !reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip < equippedWeapon.clipSize) || (controlsManager.shooting && !reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip <= 0)){
            reloading = true;
            reloadingTimer = equippedWeapon.reloadTime;
            objectsInClipText.text = "--/" + equippedWeapon.clipSize;
            serverEvents.sendEvent("ue", "sound", equippedWeapon.reloadSound + "~" + equippedWeapon.transform.position + "~1~1");
        }

        reloadingTimer -= Time.deltaTime;
        if(reloading && reloadingTimer <= 0 && equippedWeapon.objectsInClip < equippedWeapon.clipSize){
            reloading = false;
            equippedWeapon.objectsInClip = equippedWeapon.clipSize;
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + equippedWeapon.clipSize;
        }

        if(controlsManager.shooting && equippedWeapon.objectsInClip > 0 && equippedWeapon.cooldownTimer <= 0 && ableToShoot){
            if(!equippedWeapon.automatic){
                ableToShoot = false;
            }
            reloading = false;
            equippedWeapon.objectsInClip -= 1;
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + equippedWeapon.clipSize;
            equippedWeapon.cooldownTimer = equippedWeapon.cooldown;
            
            if(equippedWeapon.projectileID == 3){ //only for bullets
                projectileManager.createProjectile(0, 0, equippedWeapon.damage, cam.transform.position, cam.transform.forward * equippedWeapon.bulletTravelSpeed);
            }
            if(equippedWeapon.projectileID == 6){ //only for shotgunShells
                projectileManager.createProjectile(0, 5, equippedWeapon.damage, cam.transform.position, cam.transform.forward * equippedWeapon.bulletTravelSpeed);
            }

            serverEvents.sendEvent("ue", "pr", equippedWeapon.projectileID + "~" + equippedWeapon.damage + "~" + cam.transform.position + "~" + cam.transform.forward * equippedWeapon.bulletTravelSpeed + "~" + equippedWeapon.shootSound + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
            //serverEvents.sendEvent("ue", "sound",  + "~" + equippedWeapon.transform.position + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
        }

        /*if(!equippedGun.flash && !equippedGun.smoke && !equippedGun.granade){
            if(controlsManager.shooting && equippedGun.cooldownTimer <= 0f && equippedGun.bulletsInClip > 0){

                //events
                serverEvents.sendEvent("universalEvent", "sound", equippedGun.shootSound + "~" + equippedGun.transform.position + "~1~1");
                serverEvents.sendEvent("universalEvent", "pr", cam.transform.position + "~" + gunContainer.transform.rotation + "~" + equippedGun.bulletTravelSpeed); //need to change rotation and speed to velocity (3d)

                reloading = false;
                equippedGun.bulletsInClip -= 1;
                bulletsInClipText.text = equippedGun.bulletsInClip + "/" + equippedGun.clipSize;
                equippedGun.cooldownTimer = equippedGun.cooldown;
                
                //Physics.Raycast(cam.transform.position + cam.transform.forward * equippedGun.gunLength, cam.transform.forward, out hit, Mathf.Infinity, aimableMask);
                //bullet
                GameObject bullet = Instantiate(bulletPrefab, cam.transform.position + cam.transform.forward * equippedGun.gunLength * 1.5f, equippedGun.transform.rotation);
                GameObject fakeBullet = Instantiate(fakebulletPrefab, equippedGun.transform.position + equippedGun.transform.forward * equippedGun.gunLength, equippedGun.transform.rotation);
                bullet.GetComponent<BulletScript>().goTo(equippedGun.bulletTravelSpeed, serverEvents, equippedGun.damage, true, fakeBullet);
            }
            

            if(reloading && reloadingTimer <= 0f){
                reloading = false;
                equippedGun.bulletsInClip = equippedGun.clipSize;
                bulletsInClipText.text = equippedGun.bulletsInClip + "/" + equippedGun.clipSize;
            }

            if(controlsManager.reloading && equippedGun.bulletsInClip < equippedGun.clipSize && reloadingTimer <= 0f){
                reloading = true;
                bulletsInClipText.text = "--/" + equippedGun.clipSize;
                serverEvents.sendEvent("universalEvent", "sound", equippedGun.reloadSound + "~" + equippedGun.transform.position + "~1~1");
                reloadingTimer = equippedGun.reloadTime;
            }
        }
        else if(controlsManager.shooting && equippedGun.cooldownTimer <= 0f){
            if(equippedGun.flash){
                equippedGun.cooldownTimer = equippedGun.cooldown;
                serverEvents.sendEvent("ue", "flash", cam.transform.position + cam.transform.forward * equippedGun.gunLength + "~" + cam.transform.forward * equippedGun.bulletTravelSpeed);
            }
            else if(equippedGun.granade){
                equippedGun.cooldownTimer = equippedGun.cooldown;
                serverEvents.sendEvent("ue", "granade", cam.transform.position + cam.transform.forward * equippedGun.gunLength + "~" + cam.transform.forward * equippedGun.bulletTravelSpeed);
            }
        }*/
    }
}
