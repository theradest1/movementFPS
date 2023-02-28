using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileFunctions : MonoBehaviour
{
    //just a bunch of small functions to make projectile programming and other things faster and more readable
    //also has references to other scripts to stay clean
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public PlayerManager playerManager;
    public SoundManager soundManager;
    public WeaponManager weaponManager;
    public Collider playerColl;
    public GameObject playerCam;
    public Image flashImage;

    private void Start() {
        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        playerColl = GameObject.Find("Player").GetComponent<Collider>();
    }

    public void Explosion(Vector3 pos, float radius, float damage, float force, bool falloffDamage, LayerMask stopFrom, int senderID){
        if(!Physics.Raycast(pos, playerCam.transform.position - pos, Vector3.Distance(pos, playerCam.transform.position), stopFrom)){
            if(Vector3.Distance(pos, playerCam.transform.position) <= radius){
                if(falloffDamage){
                    damage *= 1 - Vector3.Distance(pos, playerCam.transform.position)/radius;
                }
                if(playerManager.health > damage){
                    serverEvents.sendEventFromOther(senderID, "ue", "d", serverComm.ID + "~" + damage);
                    playerManager.health -= damage;
                }
                else{
                    serverEvents.sendEventFromOther(senderID, "ue", "death", serverComm.ID + "");
                }
            }
        }
    }
}
