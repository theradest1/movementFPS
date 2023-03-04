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
    ControlsManager controlsManager;
    WeaponManager weaponManager;
    GameObject deathMenu;
    GameObject playerCam;
    movement movementScript;
    Look look;
    InGameGUIManager inGameGUIManager;

    Slider healthSlider;
    Image flashImage;

    public float health;
    public float maxHealth = 100f;
    public float flashRecovery;
    public List<GameObject> spawnPoints;
    public float timeBeforeHeal;
    public float healRate;
    public float healCooldown;
    public TMP_Dropdown mainDropdown;
    public TMP_Dropdown secondaryDropdown;
    public TMP_Dropdown toolDropdown;
    public TMP_Dropdown classDropdown;
    public GameObject killer;
    Collider coll;
    Rigidbody rb;
    public ClassInfo currentClass;

    public TextMeshProUGUI classHealthText;
    public TextMeshProUGUI classDamageText;
    public TextMeshProUGUI classAmmoText;
    public TextMeshProUGUI classToolText;
    public TextMeshProUGUI classSpeedText;
    public TextMeshProUGUI classReloadText;
    public TextMeshProUGUI classFireRateText;

    void Start(){
        playerCam = GameObject.Find("Main Camera");
        inGameGUIManager = GameObject.Find("manager").GetComponent<InGameGUIManager>();
        look = GameObject.Find("Main Camera").GetComponent<Look>();
        movementScript = this.gameObject.GetComponent<movement>();
        coll = this.gameObject.GetComponent<Collider>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
        deathMenu = GameObject.Find("deathMenu");
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        healthText = GameObject.Find("healthText").GetComponent<TextMeshProUGUI>();
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        healthSlider = GameObject.Find("health").GetComponent<Slider>();
        flashImage = GameObject.Find("flash image").GetComponent<Image>();

        currentClass = GameObject.Find(PlayerPrefs.GetString("Class", "Guy")).GetComponent<ClassInfo>();
        Debug.Log("loaded " + PlayerPrefs.GetString("Class", "Guy"));
        Debug.Log(GameObject.Find(PlayerPrefs.GetString("Class", "Guy")).name);

        //spawn();
        InvokeRepeating("heal", 0, healRate);
        changeHealth(0f);
        updateClassStats();
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

    private void Update() {
        if(controlsManager.deathMenuControlls){
            transform.position = killer.transform.position;
            transform.rotation = killer.transform.rotation;
        }

        healCooldown -= Time.deltaTime;
        if(flashImage.color.a > 0){
            flashImage.color = new Color(1, 1, 1, flashImage.color.a - flashRecovery * Time.deltaTime);
        }
        
    }

    public void spawn(){
        Cursor.lockState = CursorLockMode.Locked;
        look.camRotX = 0;
        movementScript.gravity = -0.07f;
        coll.enabled = true;
        rb.useGravity = true;
        transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].transform.position + Vector3.up;
        deathMenu.SetActive(false);
        controlsManager.deathMenuControlls = false;

        weaponManager.setWeapons(new List<string> {mainDropdown.options[mainDropdown.value].text, secondaryDropdown.options[secondaryDropdown.value].text, toolDropdown.options[toolDropdown.value].text});
        setClass(GameObject.Find(classDropdown.options[classDropdown.value].text).GetComponent<ClassInfo>());
    }

    void setClass(ClassInfo classToSet){
        //Debug.Log(classToSet.gameObject.name);
        movementScript.currentClass = classToSet;
        weaponManager.currentClass = classToSet;
        currentClass = classToSet;
        weaponManager.resetAllWeapons();
        maxHealth = classToSet.health;
        health = maxHealth;
        changeHealth(0f);
        serverEvents.sendEvent("ue", "setClass", classToSet.gameObject.name);
        serverEvents.sendEvent("ue", "setHealth", health + "~" + healCooldown);

        PlayerPrefs.SetString("Class", classToSet.gameObject.name);
        PlayerPrefs.Save();
        Debug.Log("saved " + classToSet.gameObject.name);
    }

    public void updateClassStats(){
        ClassInfo selectedClass = GameObject.Find(classDropdown.options[classDropdown.value].text).GetComponent<ClassInfo>();
        classHealthText.text = selectedClass.health + "HP";
        classDamageText.text = selectedClass.damageMult * 100 + "%";
        classAmmoText.text = selectedClass.ammoCapacityMult * 100 + "%";
        classToolText.text = selectedClass.toolCapacityMult * 100 + "%";
        classSpeedText.text = selectedClass.speedMult * 100 + "%";
        classReloadText.text = selectedClass.reloadSpeedMult * 100 + "%";
        classFireRateText.text = Mathf.Round(200 - selectedClass.gunFireSpeedMult * 100) + "%";
    }

    public void death(int killerID){
        flashImage.color = new Color(1, 1, 1, 0);
        Cursor.lockState = CursorLockMode.None;
        look.camRotX = 0;
        movementScript.gravity = 0f;
        rb.velocity = Vector3.zero;
        coll.enabled = false;
        rb.useGravity = false;
        if(killerID != serverComm.ID){
            killer = GameObject.Find(killerID + "");
        }
        else{
            killer = this.gameObject;
        }
        deathMenu.SetActive(true);
        controlsManager.deathMenuControlls = true;
        //transform.position = new Vector3(0f, -9f, 0f);
    }

    public void changeHealth(float subbedHealth){
        health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
        healthSlider.value = health/maxHealth;
        healthText.text = Mathf.Round(health) + "/" + maxHealth;
        if(subbedHealth > 0){
            healCooldown = currentClass.healCooldown;
        }
    }
}
