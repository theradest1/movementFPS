using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolRefill : MonoBehaviour
{
    public float spinSpeed;
    WeaponManager weaponManager;
    private void Update() {
        transform.Rotate(spinSpeed * Time.deltaTime, 0f, 0f);
    }
    public void setInfo(WeaponManager _weaponManager){
        weaponManager = _weaponManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        string objectName = other.gameObject.name;
        if(objectName == "Player" || objectName == "head" || objectName == "body"){
            if(objectName == "Player"){
                //if(weaponManager.refillTool()){
                //    Destroy(this.gameObject);
                //    Debug.Log("Collected tool refill");
                //}
            }
            else{
                Destroy(this.gameObject);
            }
        }
    }
}
