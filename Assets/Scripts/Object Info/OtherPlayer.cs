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
    public float health = 100f;
    [HideInInspector]
    public float maxHealth = 100f;

    public void setUsername(string usrname){
        usernameText.text = usrname;
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;
    }

    void Start(){
        coll = this.gameObject.GetComponent<Collider>();
        playerCam = GameObject.Find("Main Camera");
        changeHealth(0f);
    }

    void Update(){
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
