using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [Header("Debug:")]
    public int equippedAbility; //dash, heal
    public float cooldownTimer;
    [Header("References:")]
    public ControlsManager controlsManager;
    public Rigidbody playerRB;
    public GameObject playerCam;
    public ProjectileFunctions projectileFunctions;
    public ServerComm serverComm;
    Weapons weapons;

    [Header("Dash:")]
    public float dashTime;
    public float dashSpeed;
    public bool dashing;
    public float dashCooldown;

    [Header("Firerate Boost (not server controlled):")]
    public float quickWeaponCooldown;
    public float quickWeaponAmount;
    public float quickWeaponTime;

    [Header("Instant Reload (not server controlled):")]
    public float instantReloadCooldown;


    public void setVars(string[] vars){
        dashTime = float.Parse(vars[1]);
        dashSpeed = float.Parse(vars[2]);
        dashCooldown = float.Parse(vars[3]);
    }

    public void resetAll(){
        cooldownTimer = 0;
    }

    public void setAbility(int abilityID){
        resetAll();
        equippedAbility = abilityID;
    }

    private void Start()
    {
        weapons = GameObject.Find("Player").GetComponent<Weapons>();
    }

    public IEnumerator dash(){
        cooldownTimer = dashCooldown + dashTime;
        float startTime = Time.time;
        while(Time.time - startTime <= dashTime){
            playerRB.MovePosition(playerRB.position + controlsManager.moveDirection.y * dashSpeed * playerCam.transform.forward * Time.deltaTime + controlsManager.moveDirection.x * dashSpeed * playerCam.transform.right * Time.deltaTime);
            yield return new WaitForEndOfFrame(); 
        }
    }

    public IEnumerator firerateBoost(){
        cooldownTimer = quickWeaponCooldown + quickWeaponTime;
        float startTime = Time.time;
        while(Time.time - startTime <= quickWeaponTime){
            weapons.cooldownTimer -= quickWeaponAmount * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public void instantReload(){
        cooldownTimer = instantReloadCooldown;
        weapons.reloading = false;
        weapons.objectsInClip = weapons.equippedWeapon.clipSize;
        weapons.updateGUI();
    }

    public float getCooldownPercentage(){
        if(equippedAbility == 0){
            return cooldownTimer/dashCooldown;
        }
        else if(equippedAbility == 1){
            return cooldownTimer/quickWeaponCooldown;
        }
        else if(equippedAbility == 2){
            return cooldownTimer/instantReloadCooldown;
        }
        else{
            Debug.Log("Cooldown percent hasnt been set for ability with ID " + equippedAbility);
        }
        return 1;
    }

    public void use(){
        if(equippedAbility == 0){
            StartCoroutine(dash());
        }
        else if(equippedAbility == 1){
            StartCoroutine(firerateBoost());
        }
        else if(equippedAbility == 2){
            instantReload();
        }
        else{
            Debug.Log("Ability with ID " + equippedAbility + " not set up");
        }
    }

    private void Update() {
        cooldownTimer -= Time.deltaTime;
        if(controlsManager.abilityUse && cooldownTimer <= 0){
            use();
        }
    }
}
