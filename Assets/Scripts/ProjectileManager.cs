using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    ServerEvents serverEvents;
    ServerComm serverComm;

    WeaponManager weaponManager;
    public List<GameObject> projectiles;


    private void Start() {
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
                newProjectile.GetComponent<Flash>().setInfo(velocity);
            }
            if(projectileID == 2){
                newProjectile.GetComponent<Granade>().setInfo(velocity, damage);
            }
            if(projectileID == 3){
                newProjectile.GetComponent<FakeBullet>().setInfo(velocity, senderID);
            }
        }
    }
}
