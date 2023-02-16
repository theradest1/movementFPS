using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class WeaponManager : MonoBehaviour
{
    public ControlsManager controlsManager;
    public List<ProjectileInfo> weapons;
    ProjectileInfo equippedWeapon;
    GameObject cam;
    GameObject player;
    ServerEvents serverEvents;
    GameObject weaponContainer;
    SoundManager soundManager;
    TextMeshProUGUI objectsInClipText;
    public LayerMask playerMask;
    public LayerMask aimableMask;
    public float weaponRotationSpeed;
    public float weaponTravelSpeed;
    public float reloadingTimer;
    public bool reloading;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        weaponContainer = GameObject.Find("guns");
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        objectsInClipText = GameObject.Find("ammo left").GetComponent<TextMeshProUGUI>();

        //equippedGun = guns[0];
        changeWeapon(1);
    }

    public void changeWeapon(int newWeapon){
        if(newWeapon <= weapons.Count){
            if(equippedWeapon != null){
                equippedWeapon.gameObject.SetActive(false);
            }
            reloading = false;
            
            equippedWeapon = guns[newWeapon - 1];
            equippedWeapon.gameObject.SetActive(true);
            
            if(equippedWeapon.cooldown < equippedWeapon.equipCooldown){
                equippedWeapon.cooldown = equippedWeapon.equipCooldown;
            }
            
            Debug.Log("Changed to " + equippedWeapon.gameObject.name);
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + equippedWeapon.clipSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        weaponContainer.transform.position = Vector3.Lerp(weaponContainer.transform.position, cam.transform.position, weaponTravelSpeed * Time.deltaTime);
        weaponContainer.transform.rotation = Quaternion.Slerp(weaponContainer.transform.rotation, player.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        equippedWeapon.transform.rotation = Quaternion.Slerp(equippedWeapon.transform.rotation, cam.transform.rotation, weaponRotationSpeed * Time.deltaTime);

        if(controlsManager.shooting && equippedWeapon.objectsInClip > 0 && equippedWeapon.cooldownTimer <= 0){
            serverEvents.sendEvent("ue", "pr", cam.transform.position + "~" + gunContainer.transform.rotation + "~" + equippedGun.bulletTravelSpeed);
        }

        /*if(!equippedGun.flash && !equippedGun.smoke && !equippedGun.granade){
            if(controlsManagerScript.shooting && equippedGun.cooldownTimer <= 0f && equippedGun.bulletsInClip > 0){

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

            if(controlsManagerScript.reloading && equippedGun.bulletsInClip < equippedGun.clipSize && reloadingTimer <= 0f){
                reloading = true;
                bulletsInClipText.text = "--/" + equippedGun.clipSize;
                serverEvents.sendEvent("universalEvent", "sound", equippedGun.reloadSound + "~" + equippedGun.transform.position + "~1~1");
                reloadingTimer = equippedGun.reloadTime;
            }
        }
        else if(controlsManagerScript.shooting && equippedGun.cooldownTimer <= 0f){
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
