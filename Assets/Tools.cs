using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    [Header("References:")]
    public ProjectileFunctions projectileFunctions;
    public ServerComm serverComm;
    public int equippedTool; //grapple, heal, double jump
    public ControlsManager controlsManager;
    public GameObject grappleIndicator;
    public Grapple grapple;

    [Header("Controlled by server:")]
    public int maxChargesGrapple;
    public float healTime;
    public float healTotal;
    public int healSteps;
    public float healCooldown;
    public int maxChargesHeal;

    [Header("Debug:")]
    public float cooldownTimer;
    public int charges;

    public void setVars(string[] vars){
        maxChargesGrapple = int.Parse(vars[1]);
        healTime = float.Parse(vars[2]);
        healSteps = int.Parse(vars[3]);
        healTotal = float.Parse(vars[4]);
        healCooldown = float.Parse(vars[5]);
        maxChargesHeal = int.Parse(vars[6]);
    }

    private void LateUpdate()
    {
        if(equippedTool == 0){
            grappleIndicator.transform.position = grapple.getLookPos();
        }
    }

    public IEnumerator heal(){
        cooldownTimer = healCooldown + healTime;
        for(int step = 0; step < healSteps; step++){
            projectileFunctions.triggerDamage(null, -healTotal/healSteps, serverComm.ID);
            yield return new WaitForSeconds(healTime/healSteps);
        }
    }

    public void setTool(int newTool){
        equippedTool = newTool;
        if(equippedTool == 0){
            grappleIndicator.SetActive(true);
        }
        else{
            grappleIndicator.SetActive(false);
        }
        resetAll();
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if(equippedTool == 0){
            if(grapple.grappling && !controlsManager.toolUse){
                grapple.detach();
            }
            else if(!grapple.grappling && controlsManager.toolUse && charges > 0){
                if(grapple.attach()){
                    charges--;
                }
            }
        }
        else if(equippedTool == 1 && controlsManager.toolUse && cooldownTimer <= 0 && charges > 0){
            charges--;
            StartCoroutine(heal());
        }
    }

    public void resetAll(){
        cooldownTimer = 0f;
        if(equippedTool == 0){
            charges = maxChargesGrapple;
        }
        else{
            charges = maxChargesHeal;
        }
    }

    public float getCooldownPercentage(){
        if(equippedTool == 0){
            return 0f;
        }
        else if(equippedTool == 1){
            return cooldownTimer/(healCooldown + healTime);
        }
        return 1;
    }

    public bool collectCharge(){
        if(equippedTool == 0 && charges < maxChargesGrapple){
            charges++;
            return true;
        }
        else if(equippedTool == 1 && charges < maxChargesHeal){
            charges++;
            return true;
        }
        return false;
    }
}
