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

    void Start(){
        onHealthChange(100f);
    }

    public void onHealthChange(float newHealth){
        health = newHealth;
        healthSlider.value = health/maxHealth;
        healthText.text = Mathf.Round(health) + "/100";
    }
}
