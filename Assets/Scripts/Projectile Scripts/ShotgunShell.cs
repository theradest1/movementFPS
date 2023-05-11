using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunShell : MonoBehaviour
{
    public float spread;
    public float bullets;
    //float damagePerBullet;
    public GameObject bulletPrefab;
    public GameObject fakeBulletPrefab;
    //ProjectileFunctions projectileFunctions;

    public void setInfo(float damagePerBullet, Vector3 fouxBulletPos, Vector3 velocity, ProjectileFunctions projectileFunctions, bool fake){
        GameObject newBullet;
        if(!fake){
            for(int i = 0; i < bullets; i++){
                newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                newBullet.GetComponent<Bullet>().setInfo(velocity + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)), damagePerBullet, fouxBulletPos, projectileFunctions);
            }

            WeaponInfo equippedWeapon = projectileFunctions.weapons.equippedWeapon;
            projectileFunctions.serverEvents.sendEvent("ue", "pr", 6 + "~" + 0 + "~" + fouxBulletPos + "~" + velocity + "~" + equippedWeapon.shootSound + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
        }
        else{
            for(int i = 0; i < bullets; i++){
                newBullet = Instantiate(fakeBulletPrefab, transform.position, Quaternion.identity);
                newBullet.GetComponent<FakeBullet>().setInfo(velocity + new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), Random.Range(-spread, spread)), 0);
                //newBullet.GetComponent<Bullet>().setInfo(velocity, damagePerBullet, fouxBulletPos, projectileFunctions);
            }
        }
        Destroy(this.gameObject);
    }
}
