using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    //data other scripts use
    [HideInInspector]
    public List<string> replayData = new List<string>();
    [HideInInspector]
    public float invincibilityTimer = 0f;
    public float health;
    public float maxHealth;
    public float healRate;
    public float healCooldown;
    public Vector3 direction;
    public int currentMapVote = -1;
    [HideInInspector]
    public TextMeshProUGUI scoreboardPeice; 


    public Vector3 targetPosition;
    public Vector3 pastTargetPosition;
    public Quaternion targetRotation;
    public Quaternion pastTargetRotation;
    public string username;
    public int kills;
    public int deaths;

    
    //scripts
    PlayerManager playerManager;
    ReplayManager replayManager;
    ServerEvents serverEvents;
    
    
    [Header("References: ")]
    public GameObject playerCam;
    public TextMeshProUGUI usernameText;
    public Canvas usernameCanvas;
    public Slider healthSlider;
    public Collider bodyColl;
    public Collider headColl;
    public SkinnedMeshRenderer bodyRenderer;


    public void setUsername(string givenUsername){
        usernameText.text = givenUsername;
        username = givenUsername;
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;
        Debug.Log(healthSlider.value);

        if(subbedHealth > 0){
            healCooldown = playerManager.timeBeforeHeal;
        }
    }

    void Start(){
        targetPosition = Vector3.zero;
        pastTargetPosition = Vector3.zero;
        targetRotation = Quaternion.identity;
        pastTargetRotation = Quaternion.identity;

        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        replayManager = GameObject.Find("manager").GetComponent<ReplayManager>();
        playerManager = GameObject.Find("manager").GetComponent<ProjectileFunctions>().playerManager;
        playerCam = GameObject.Find("manager").GetComponent<ProjectileFunctions>().playerCam;
        healRate = playerManager.healRate;
        maxHealth = playerManager.maxHealth;
        health = 100f;
        healRate = playerManager.healRate;
        changeHealth(0f);
        InvokeRepeating("heal", 0, .1f);
        for(int i = 0; i < replayManager.tickRate * replayManager.replayTime; i++){
            replayData.Add("");
        }
        //InvokeRepeating("storeReplayData", 0f, 1f/(float)replayManager.tickRate);
    }

    public void storeReplayData(int currentTick){
        replayData[currentTick] = gameObject.name + "~" + transform.position + "~" + transform.eulerAngles;
    }

    void heal(){
        if(healCooldown <= 0 && health < maxHealth){
            if(health > maxHealth){
                health = maxHealth;
                changeHealth(0);
            }
            else{
                changeHealth(-healRate);
            }
        }
    }

    public void changeScoreboard(int killsToAdd, int deathsToAdd){
        kills += killsToAdd;
        deaths += deathsToAdd;
        scoreboardPeice.text = kills + "/" + deaths;
    }

    void Update(){
        healCooldown -= Time.deltaTime;
        usernameCanvas.gameObject.transform.LookAt(playerCam.transform);

        invincibilityTimer -= Time.deltaTime;

        if(invincibilityTimer > 0f){
            bodyColl.enabled = false;
            headColl.enabled = false;
        }
        else{
            bodyColl.enabled = true;
            headColl.enabled = true;
        }

        transform.position = Vector3.Lerp(pastTargetPosition, targetPosition, serverEvents.percentDone);
        transform.rotation = Quaternion.Slerp(pastTargetRotation, targetRotation, serverEvents.percentDone);
    }
}
