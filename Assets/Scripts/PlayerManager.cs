using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public float health;
    public float maxHealth = 100f;
    public TextMeshProUGUI healthText;
    public Slider healthSlider;
    public ServerEvents serverEvents;
    public ServerComm serverComm;

    void Start(){
        changeHealth(0f);
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;
        healthText.text = Mathf.Round(health) + "/100";
        if(health == 0f){
            transform.position = new Vector3(Random.Range(-20f, 20f), 20f, Random.Range(-20f, 20f));
            serverEvents.sendEvent("universalEvent", "damage", serverComm.ID + "~" + -100);
        }
    }
}
