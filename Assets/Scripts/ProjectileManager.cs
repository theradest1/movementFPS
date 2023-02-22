using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    ServerEvents serverEvents;
    ServerComm serverComm;
    public List<GameObject> projectiles;


    private void Start() {
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
    }
    public void createProjectile(int senderID, int projectileID, float damage, Vector3 initialPos, Vector3 velocity){
        GameObject newProjectile = Instantiate(projectiles[projectileID], initialPos, Quaternion.identity);
        if(projectileID == 0){
            newProjectile.GetComponent<Bullet>().setInfo(senderID, damage, serverEvents);
        }
        if(projectileID == 1){
            //newProjectile.GetComponent<Flash>().setInfo();
        }
        if(projectileID == 2){
            newProjectile.GetComponent<Granade>().setInfo(damage);
        }
        if(projectileID == 3 && senderID != serverComm.ID){
            newProjectile.GetComponent<FakeBullet>().setInfo(senderID);
        }
    }
}
