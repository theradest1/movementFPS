using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    TextMeshProUGUI healthText;
    ServerEvents serverEvents;
    ServerComm serverComm;
    Slider healthSlider;
    Image flashImage;

    public float health;
    public float maxHealth = 100f;
    public float flashRecovery;
    public List<GameObject> spawnPoints;
    public float timeBeforeHeal;
    public float healRate;
    public float healCooldown;

    void Start(){
        healthText = GameObject.Find("healthText").GetComponent<TextMeshProUGUI>();
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        healthSlider = GameObject.Find("health").GetComponent<Slider>();
        flashImage = GameObject.Find("flash image").GetComponent<Image>();

        spawn();
        InvokeRepeating("heal", 0, healRate);
        changeHealth(0f);
    }

    void heal(){
        if(healCooldown <= 0 && health < maxHealth){
            changeHealth(-1);
        }
    }

    private void Update() {
        healCooldown -= Time.deltaTime;
        if(flashImage.color.a > 0){
            flashImage.color = new Color(1, 1, 1, flashImage.color.a - flashRecovery * Time.deltaTime);
        }
    }

    public void spawn(){
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position + Vector3.up;
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;
        healthText.text = Mathf.Round(health) + "/" + maxHealth;
        if(subbedHealth > 0){
            healCooldown = timeBeforeHeal;
        }
    }
}
