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

    // Start is called before the first frame update
    void Start()
    {
        equippedGun = guns[0];
    }

    // Update is called once per frame
    void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if(controlsManagerScript.shooting && cooldownTimer <= 0f){
            cooldownTimer = equippedGun.cooldown;
            RaycastHit hit;
            if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, playerMask)){
                Debug.Log("Hit player with ID " + hit.transform.gameObject.name);
                serverEvents.sendEvent("universalEvent", "damage", hit.transform.gameObject.name + "~" + equippedGun.damage);
            }
        }
    }
}
