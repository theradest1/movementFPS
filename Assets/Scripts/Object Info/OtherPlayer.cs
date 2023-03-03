using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    public GameObject playerCam;

    public TextMeshProUGUI usernameText;
    public Canvas usernameCanvas;
    public Slider healthSlider;

    public float invincibilityTimer = 0f;

    Collider coll;
    
    //[HideInInspector]
    public float health;
    public float maxHealth;

    //[HideInInspector]

    public float healRate;
    PlayerManager playerManager;
    public ClassInfo currentClass;
    public float healCooldown;

    public void setUsername(string usrname){
        usernameText.text = usrname;
    }

    public void setClass(string classToSet){
        currentClass = GameObject.Find(classToSet).GetComponent<ClassInfo>();
        maxHealth = currentClass.health;
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;

        if(subbedHealth > 0){
            healCooldown = currentClass.healCooldown;
        }
    }

    void Start(){
        playerManager = GameObject.Find("manager").GetComponent<ProjectileFunctions>().playerManager;
        currentClass = GameObject.Find("Guy").GetComponent<ClassInfo>();
        playerCam = GameObject.Find("manager").GetComponent<ProjectileFunctions>().playerCam;
        healRate = playerManager.healRate;
        maxHealth = currentClass.health;
        health = 100f;

        coll = this.gameObject.GetComponent<Collider>();
        changeHealth(0f);
        InvokeRepeating("heal", 0, healRate);
    }

    void heal(){
        if(healCooldown <= 0 && health < maxHealth){
            if(health + currentClass.healAmount > maxHealth){
                health = maxHealth;
                changeHealth(0);
            }
            else{
                changeHealth(-currentClass.healAmount);
            }
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
