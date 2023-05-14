using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileManager : MonoBehaviour
{
    Weapons weapons;
    public ServerEvents serverEvents;
    ProjectileFunctions projectileFunctions;
    public List<GameObject> projectiles;

    private void Start() {
        projectileFunctions = GameObject.Find("manager").GetComponent<ProjectileFunctions>();
        weapons = GameObject.Find("Player").GetComponent<Weapons>();
        
        Physics.IgnoreLayerCollision(9, 9, true);
    }
    public void createProjectile(int senderID, int projectileID, float damage, Vector3 initialPos, Quaternion initialRotation, Vector3 velocity, Vector3 fouxInitialPos, bool sendServerEvent = false, int shootSound = 0, float shootVolume = 0, float shootPitch = 0){
        if((projectileID == 3 && senderID != projectileFunctions.serverComm.ID) || (projectileID == 6 && senderID != projectileFunctions.serverComm.ID) || (projectileID != 3 && projectileID != 6)){
            if(!sendServerEvent){
                GameObject newProjectile = Instantiate(projectiles[projectileID], initialPos, initialRotation);
                if(projectileID == 0){
                    newProjectile.GetComponent<Bullet>().setInfo(velocity, damage, fouxInitialPos, projectileFunctions);
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
                    newProjectile.GetComponent<Smoke>().setInfo(velocity, projectileFunctions);
                }
                if(projectileID == 5){
                    newProjectile.GetComponent<ShotgunShell>().setInfo(damage, fouxInitialPos, velocity, projectileFunctions, false);
                    //newProjectile.GetComponent<ShotgunShell>().setInfo(damage, weapons.equippedWeapon.transform.position + weapons.equippedWeapon.startDistance * weapons.equippedWeapon.transform.forward, velocity, projectileFunctions, true);
                }
                if(projectileID == 6){
                    Debug.Log("Created fake shotgun bullets");
                    newProjectile.GetComponent<ShotgunShell>().setInfo(damage, fouxInitialPos, velocity, projectileFunctions, true);
                }
            }
            else{
                serverEvents.sendEvent("ue", "pr", projectileID + "~" + damage + "~" + initialPos + "~" + velocity + "~" + shootSound + "~" + shootVolume + "~" + shootPitch);
            }
        }
    }
}
