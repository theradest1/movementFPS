using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour
{
    WeaponManager weaponManager;
    ProjectileFunctions projectileFunctions;
    public List<GameObject> projectiles;

    private void Start() {
        projectileFunctions = GameObject.Find("manager").GetComponent<ProjectileFunctions>();
        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
    }
    public void createProjectile(int senderID, int projectileID, float damage, Vector3 initialPos, Vector3 velocity){
        if((projectileID == 3 && senderID != projectileFunctions.serverComm.ID) || projectileID != 3){
            GameObject newProjectile = Instantiate(projectiles[projectileID], initialPos, Quaternion.identity);
            if(projectileID == 0){
                newProjectile.GetComponent<Bullet>().setInfo(velocity, damage, weaponManager.equippedWeapon.transform.position + weaponManager.equippedWeapon.startDistance * weaponManager.equippedWeapon.transform.forward, projectileFunctions);
            }
            if(projectileID == 1){
                newProjectile.GetComponent<Flash>().setInfo(velocity, projectileFunctions);
            }
            if(projectileID == 2){
                newProjectile.GetComponent<Granade>().setInfo(velocity, damage, senderID, projectileFunctions);
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
