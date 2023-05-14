using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [Header("References:")]
    public ControlsManager controlsManager;
    public Rigidbody playerRB;
    public GameObject playerCam;
    public ProjectileFunctions projectileFunctions;
    public ServerComm serverComm;

    [Header("Dash:")]
    public float dashTime;
    public float dashSpeed;
    public bool dashing;
    public float dashCooldown;

    [Header("Heal")]
    public float healTime;
    public float healTotal;
    public int healSteps;
    public float healCooldown;

    [Header("Debug:")]
    public int equippedAbility; //dash, heal
    public float cooldownTimer;

    public void setVars(string[] vars){
        dashTime = float.Parse(vars[1]);
        dashSpeed = float.Parse(vars[2]);
        dashCooldown = float.Parse(vars[3]);

        healTime = float.Parse(vars[4]);
        healTotal = float.Parse(vars[5]);
        healSteps = int.Parse(vars[6]);
        healCooldown = float.Parse(vars[7]);
    }

    public void resetAll(){
        cooldownTimer = 0;
    }

    public void setAbility(int abilityID){
        resetAll();
        equippedAbility = abilityID;
    }

    public IEnumerator dash(){
        cooldownTimer = dashCooldown + dashTime;
        float startTime = Time.time;
        while(Time.time - startTime <= dashTime){
            playerRB.MovePosition(playerRB.position + controlsManager.moveDirection.y * dashSpeed * playerCam.transform.forward * Time.deltaTime + controlsManager.moveDirection.x * dashSpeed * playerCam.transform.right * Time.deltaTime);
            yield return new WaitForEndOfFrame(); 
        }
    }

    public float getCooldownPercentage(){
        if(equippedAbility == 0){
            return cooldownTimer/dashCooldown;
        }
        else if(equippedAbility == 1){
            return cooldownTimer/healCooldown;
        }
        return 1;
    }

    public IEnumerator heal(){
        cooldownTimer = healCooldown + healTime;
        Debug.Log("Started healing");
        for(int step = 0; step < healSteps; step++){
            projectileFunctions.triggerDamage(null, -healTotal/healSteps, serverComm.ID);
            yield return new WaitForSeconds(healTime/healSteps);
        }
        Debug.Log("Finished healing");
    }

    public void use(){
        Debug.Log("Used ability with ID " + equippedAbility);
        if(equippedAbility == 0){
            StartCoroutine(dash());
        }
        if(equippedAbility == 1){
            StartCoroutine(heal());
        }
    }

    private void Update() {
        cooldownTimer -= Time.deltaTime;
        if(controlsManager.abilityUse && cooldownTimer <= 0){
            use();
        }
    }
}
