using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwables : MonoBehaviour
{
    [Header("References:")]
    public ControlsManager controlsManager;
    public ProjectileManager projectileManager;
    public ServerComm serverComm;
    public Transform playerCam;
    public Rigidbody playerRB;

    [Header("Settings:")]
    public List<ThrowableInfo> throwableInfos;

    [Header("Debug:")]
    public float cooldownTimer;
    public ThrowableInfo equippedThrowable;
    public int throwableCharges;
    public void resetAll(){
        Debug.Log("reset throwables");
        throwableCharges = equippedThrowable.maxCharges;
    }

    private void Update() {
        cooldownTimer -= Time.deltaTime;
        if(throwableCharges > 0 && controlsManager.throwableUse && cooldownTimer <= 0){
            use();
        }
    }

    public void setThrowable(int throwableID){
        equippedThrowable = throwableInfos[throwableID];
        throwableCharges = equippedThrowable.maxCharges;
    }

    public void use(){
        throwableCharges--;
        projectileManager.createProjectile(serverComm.ID, equippedThrowable.projectileID, equippedThrowable.damage, playerCam.position + playerCam.forward, Quaternion.identity, playerCam.forward * equippedThrowable.speed + playerRB.velocity, Vector3.zero, true, equippedThrowable.throwSound, equippedThrowable.throwVolume, equippedThrowable.throwPitch);
        cooldownTimer = equippedThrowable.cooldown;
    }

    public bool collectThrowableCharge(){
        if(throwableCharges < equippedThrowable.maxCharges){
            throwableCharges++;
            return true;
        }
        return false;
    }
}
