using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public float maxGunRotation;
    public float minAimDistance;
    public SoundManager soundManager;

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
        //gunContainer.transform.rotation = Quaternion.Slerp(gunContainer.transform.rotation, cam.transform.rotation, gunRotationSpeed * Time.deltaTime);
        gunContainer.transform.rotation = player.transform.rotation;
        if(Physics.Raycast(cam.transform.position + cam.transform.forward * minAimDistance, cam.transform.forward, out hit, Mathf.Infinity, aimableMask)){
            Quaternion rot = Quaternion.LookRotation(hit.point - equippedGun.transform.position);
            equippedGun.transform.rotation = Quaternion.Slerp(equippedGun.transform.rotation, rot, gunRotationSpeed * Time.deltaTime);
            //equippedGun.transform.localRotation = Quaternion.Euler(equippedGun.transform.localEulerAngles.x, Mathf.Clamp(equippedGun.transform.localEulerAngles.y, -maxGunRotation, maxGunRotation), equippedGun.transform.localEulerAngles.z);
        }

        cooldownTimer -= Time.deltaTime;
        if(controlsManagerScript.shooting && cooldownTimer <= 0f){
            cooldownTimer = equippedGun.cooldown;
            serverEvents.sendEvent("universalEvent", "sound", "0~" + equippedGun.transform.position + "~1~1");
            soundManager.playSound(0, equippedGun.transform.position, 1f, 1f);
            serverEvents.sendEvent("universalEvent", "spawnBullet", equippedGun.transform.position + "~" + gunContainer.transform.rotation + "~" + equippedGun.bulletTravelSpeed);
            GameObject bullet = Instantiate(bulletPrefab, equippedGun.transform.position, equippedGun.transform.rotation);
            bullet.GetComponent<BulletScript>().goTo(equippedGun.bulletTravelSpeed, serverEvents, equippedGun.damage, true);
            gunContainer.transform.rotation *= Quaternion.Euler(-equippedGun.recoilVertical, 0f, 0f);
            gunContainer.transform.rotation *= Quaternion.Euler(Random.Range(-equippedGun.recoilHorizontal, equippedGun.recoilHorizontal), 0f, 0f);
            //if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, playerMask)){
            //    Debug.Log("Hit player with ID " + hit.transform.gameObject.name);
            //    serverEvents.sendEvent("universalEvent", "damage", hit.transform.gameObject.name + "~" + equippedGun.damage);
            //}
        }
    }
}
