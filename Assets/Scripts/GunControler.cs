using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunControler : MonoBehaviour
{
    public controlsManager controlsManagerScript;

    public List<GunScript> guns;
    GunScript equippedGun;

    GameObject cam;
    GameObject player;
    ServerEvents serverEvents;
    GameObject gunContainer;
    SoundManager soundManager;
    TextMeshProUGUI bulletsInClipText;

    public GameObject bulletPrefab;
    public GameObject fakebulletPrefab;
    
    public LayerMask playerMask;
    public LayerMask aimableMask;
    public float gunRotationSpeed;
    public float gunTravelSpeed;
    public float maxGunRotation;
    public bool reloading;
    public float reloadingTimer;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        gunContainer = GameObject.Find("guns");
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        bulletsInClipText = GameObject.Find("ammo left").GetComponent<TextMeshProUGUI>();

        //equippedGun = guns[0];
        changeObject(1);
    }

    public void changeObject(int newObject){
        if(newObject <= guns.Count){
            //cooldownTimer = 0f; //exploit (switch to remove cooldown)
            reloading = false;
            if(equippedGun != null){
                equippedGun.gameObject.SetActive(false);
            }
            equippedGun = guns[newObject - 1];
            equippedGun.gameObject.SetActive(true);
            //Debug.Log("Changed to " + equippedGun.gameObject.name);
            bulletsInClipText.text = equippedGun.bulletsInClip + "/" + equippedGun.clipSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //RaycastHit hit;
        gunContainer.transform.position = Vector3.Lerp(gunContainer.transform.position, cam.transform.position, gunTravelSpeed * Time.deltaTime);
        gunContainer.transform.rotation = Quaternion.Slerp(gunContainer.transform.rotation, player.transform.rotation, gunRotationSpeed * Time.deltaTime);
        equippedGun.transform.rotation = Quaternion.Slerp(equippedGun.transform.rotation, cam.transform.rotation, gunRotationSpeed * Time.deltaTime);
        //Debug.Log(cam.transform.eulerAngles);
        //if(Physics.Raycast(cam.transform.position + cam.transform.forward * minAimDistance, cam.transform.forward, out hit, Mathf.Infinity, aimableMask)){
        //    Quaternion rot = Quaternion.LookRotation(hit.point - equippedGun.transform.position);
        //    equippedGun.transform.rotation = Quaternion.Slerp(equippedGun.transform.rotation, rot, gunRotationSpeed * Time.deltaTime);
        //    //equippedGun.transform.localRotation = Quaternion.Euler(equippedGun.transform.localEulerAngles.x, Mathf.Clamp(equippedGun.transform.localEulerAngles.y, -maxGunRotation, maxGunRotation), equippedGun.transform.localEulerAngles.z);
        //}

        equippedGun.cooldownTimer -= Time.deltaTime;
        reloadingTimer -= Time.deltaTime;

        if(!equippedGun.flash && !equippedGun.smoke && !equippedGun.granade){
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
        }
    }
}
