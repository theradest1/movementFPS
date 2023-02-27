using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour
{
    ServerEvents serverEvents;
    ServerComm serverComm;
    SoundManager soundManager;
    public Image flashImage;
    public GameObject playerCam;
    WeaponManager weaponManager;
    PlayerManager playerManager;
    public List<GameObject> projectiles;


    private void Start() {
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
    }
    public void createProjectile(int senderID, int projectileID, float damage, Vector3 initialPos, Vector3 velocity){
        if((projectileID == 3 && senderID != serverComm.ID) || projectileID != 3){
            GameObject newProjectile = Instantiate(projectiles[projectileID], initialPos, Quaternion.identity);
            if(projectileID == 0){
                newProjectile.GetComponent<Bullet>().setInfo(velocity, damage, serverEvents, weaponManager.equippedWeapon.transform.position + weaponManager.equippedWeapon.startDistance * weaponManager.equippedWeapon.transform.forward);
            }
            if(projectileID == 1){
                newProjectile.GetComponent<Flash>().setInfo(velocity, soundManager, flashImage, playerCam);
            }
            if(projectileID == 2){
                newProjectile.GetComponent<Granade>().setInfo(velocity, damage, playerManager, playerCam, senderID, serverEvents, serverComm, soundManager);
            }
            if(projectileID == 3){
                newProjectile.GetComponent<FakeBullet>().setInfo(velocity, senderID);
            }
            if(projectileID == 4){
                newProjectile.GetComponent<Smoke>().setInfo(velocity);
            }
        }
    }
}
