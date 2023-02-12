using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunControler : MonoBehaviour
{
    public controlsManager controlsManagerScript;

    public List<GunScript> guns;
    public GunScript equippedGun;

    public GameObject cam;
    public GameObject player;
    public LayerMask playerMask;
    public LayerMask aimableMask;
    public float cooldownTimer;
    public ServerEvents serverEvents;
    public GameObject gunContainer;
    public float gunRotationSpeed;
    public float gunTravelSpeed;
    public GameObject bulletPrefab;
    public GameObject fakebulletPrefab;
    public float maxGunRotation;
    public float minAimDistance;
    public SoundManager soundManager;
    public bool reloading;
    public float reloadingTimer;
    public TextMeshProUGUI bulletsInClipText;


    // Start is called before the first frame update
    void Start()
    {
        equippedGun = guns[0];
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        gunContainer.transform.position = Vector3.Lerp(gunContainer.transform.position, cam.transform.position, gunTravelSpeed * Time.deltaTime);
        gunContainer.transform.rotation = Quaternion.Slerp(gunContainer.transform.rotation, player.transform.rotation, gunRotationSpeed * Time.deltaTime);
        equippedGun.transform.rotation = Quaternion.Slerp(equippedGun.transform.rotation, cam.transform.rotation, gunRotationSpeed * Time.deltaTime);
        Debug.Log(cam.transform.eulerAngles);
        //if(Physics.Raycast(cam.transform.position + cam.transform.forward * minAimDistance, cam.transform.forward, out hit, Mathf.Infinity, aimableMask)){
        //    Quaternion rot = Quaternion.LookRotation(hit.point - equippedGun.transform.position);
        //    equippedGun.transform.rotation = Quaternion.Slerp(equippedGun.transform.rotation, rot, gunRotationSpeed * Time.deltaTime);
        //    //equippedGun.transform.localRotation = Quaternion.Euler(equippedGun.transform.localEulerAngles.x, Mathf.Clamp(equippedGun.transform.localEulerAngles.y, -maxGunRotation, maxGunRotation), equippedGun.transform.localEulerAngles.z);
        //}

        cooldownTimer -= Time.deltaTime;
        reloadingTimer -= Time.deltaTime;
        if(controlsManagerScript.shooting && cooldownTimer <= 0f && equippedGun.bulletsInClip > 0){

            //events
            serverEvents.sendEvent("universalEvent", "sound", equippedGun.shootSound + "~" + equippedGun.transform.position + "~1~1");
            serverEvents.sendEvent("universalEvent", "spawnBullet", cam.transform.position + "~" + gunContainer.transform.rotation + "~" + equippedGun.bulletTravelSpeed);

            reloading = false;
            equippedGun.bulletsInClip -= 1;
            bulletsInClipText.text = equippedGun.bulletsInClip + "/" + equippedGun.clipSize;
            cooldownTimer = equippedGun.cooldown;
            
            Physics.Raycast(cam.transform.position + cam.transform.forward * minAimDistance, cam.transform.forward, out hit, Mathf.Infinity, aimableMask);
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
}
