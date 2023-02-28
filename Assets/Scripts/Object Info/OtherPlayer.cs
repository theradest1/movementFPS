using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    GameObject playerCam;

    public TextMeshProUGUI usernameText;
    public Canvas usernameCanvas;
    public Slider healthSlider;

    public float invincibilityTimer = 0f;

    Collider coll;
    
    [HideInInspector]
    public float health;
    float maxHealth;

    
    float timeBeforeHeal;
    float healRate;
    float healCooldown;
    PlayerManager playerManager;

    public void setUsername(string usrname){
        usernameText.text = usrname;
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;

        if(subbedHealth > 0){
            healCooldown = timeBeforeHeal;
        }
    }

    void Start(){
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        healRate = playerManager.healRate;
        timeBeforeHeal = playerManager.timeBeforeHeal;
        health = playerManager.health;
        maxHealth = playerManager.maxHealth;

        coll = this.gameObject.GetComponent<Collider>();
        playerCam = GameObject.Find("Main Camera");
        changeHealth(0f);
        InvokeRepeating("heal", 0, healRate);
    }

    void heal(){
        if(healCooldown <= 0 && health < maxHealth){
            changeHealth(-1);
        }
    }

    void Update(){
        healCooldown -= Time.deltaTime;
        usernameCanvas.gameObject.transform.LookAt(playerCam.transform);

        invincibilityTimer -= Time.deltaTime;

        if(invincibilityTimer > 0f){
            coll.enabled = false;
        }
        else{
            coll.enabled = true;
        }
    }
}
