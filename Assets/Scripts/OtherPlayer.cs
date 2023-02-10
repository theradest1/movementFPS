using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public Canvas usernameCanvas;
    GameObject playerCam;
    public float health = 100f;
    public float maxHealth = 100f;
    public Slider healthSlider;

    public void setUsername(string usrname){
        usernameText.text = usrname;
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;
    }

    void Start(){
        playerCam = GameObject.Find("Main Camera");
        Debug.Log(playerCam);
        changeHealth(0f);
    }

    void Update(){
        usernameCanvas.gameObject.transform.LookAt(playerCam.transform);
    }
}
