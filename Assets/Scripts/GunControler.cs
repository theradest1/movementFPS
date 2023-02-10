using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunControler : MonoBehaviour
{
    public controlsManager controlsManagerScript;

    public List<GunScript> guns;
    public GunScript equippedGun;

    public GameObject cam;
    public LayerMask playerMask;
    public float cooldownTimer;
    public ServerEvents serverEvents;
    public GameObject gunContainer;
    public float gunRotationSpeed;
    public float gunTravelSpeed;
    public GameObject bulletPrefab;

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
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity)){
            Quaternion rot = Quaternion.LookRotation(hit.point - gunContainer.transform.position);
            gunContainer.transform.rotation = Quaternion.Slerp(gunContainer.transform.rotation, rot, gunRotationSpeed * Time.deltaTime);
        }

        cooldownTimer -= Time.deltaTime;
        if(controlsManagerScript.shooting && cooldownTimer <= 0f){
            cooldownTimer = equippedGun.cooldown;
            serverEvents.sendEvent("universalEvent", "spawnBullet", equippedGun.transform.position + "~" + gunContainer.transform.rotation + "~" + equippedGun.bulletTravelSpeed);
            GameObject bullet = Instantiate(bulletPrefab, equippedGun.transform.position, gunContainer.transform.rotation);
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
