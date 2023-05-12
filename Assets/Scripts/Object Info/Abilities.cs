using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Abilities : MonoBehaviour
{
    [Header("References:")]
    public ControlsManager controlsManager;
    public Rigidbody playerRB;
    public GameObject playerCam;

    [Header("Dash:")]
    public float dashTime;
    public float dashSpeed;
    public bool dashing;
    public float dashCooldown;

    [Header("Debug:")]
    public int equippedAbility; //dash, heal
    public float cooldownTimer;

    public void resetAll(){
        Debug.Log("reset abilities");
    }

    public IEnumerator dash(){
        float startTime = Time.time;
        while(Time.time - startTime <= dashTime){
            Debug.Log("dashing");
            playerRB.MovePosition(playerRB.position + controlsManager.moveDirection.y * dashSpeed * playerCam.transform.forward * Time.deltaTime + controlsManager.moveDirection.x * dashSpeed * playerCam.transform.right * Time.deltaTime);
            yield return new WaitForEndOfFrame(); 
        }
        cooldownTimer = dashCooldown;
    }

    public void use(){
        Debug.Log("Used ability with ID " + equippedAbility);
        if(equippedAbility == 0){
            StartCoroutine(dash());
        }
    }

    private void Update() {
        cooldownTimer -= Time.deltaTime;
        if(controlsManager.abilityUse && cooldownTimer <= 0){
            use();
        }
    }
}
