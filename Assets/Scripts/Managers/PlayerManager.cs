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
    [HideInInspector]
    public GameObject deathMenu;
    GameObject playerCam;
    movement movementScript;
    Look look;
    InGameGUIManager inGameGUIManager;
    ReplayManager replayManager;

    Slider healthSlider;
    Image flashImage;

    public float health;
    public float maxHealth = 100f;
    public float flashRecovery;
    //public List<GameObject> spawnPoints;
    public float timeBeforeHeal;
    public float healRate;
    public float healCooldown;
    public TMP_Dropdown mainDropdown;
    public TMP_Dropdown secondaryDropdown;
    public TMP_Dropdown toolDropdown;
    public TMP_Dropdown classDropdown;
    public GameObject killer;
    bool followKiller = false;
    Collider coll;
    Rigidbody rb;
    public ClassInfo currentClass;
    public MapInfo currentMap;

    public TextMeshProUGUI classHealthText;
    public TextMeshProUGUI classDamageText;
    public TextMeshProUGUI classAmmoText;
    public TextMeshProUGUI classToolText;
    public TextMeshProUGUI classSpeedText;
    public TextMeshProUGUI classReloadText;
    public TextMeshProUGUI classFireRateText;


    public TextMeshProUGUI weaponSpeedText;
    public TextMeshProUGUI weaponDamageText;
    public TextMeshProUGUI weaponFireRateText;
    public TextMeshProUGUI weaponReloadTimeText;
    public TextMeshProUGUI weaponClipSizeText;
    public TextMeshProUGUI weaponHeadshotText;
    public TextMeshProUGUI weaponNameText;

    void Start(){
        replayManager = GameObject.Find("manager").GetComponent<ReplayManager>();
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

        //updateWeaponStats(mainDropdown);
        int _class = PlayerPrefs.GetInt("Class", 0);
        classDropdown.value = _class;

        InvokeRepeating("heal", 0, healRate);
        changeHealth(0f);
        updateClassStats();

        Invoke("getWeaponChoices", .1f);
    }

    void getWeaponChoices(){
        int _main = PlayerPrefs.GetInt("Main", 0);
        int _second = PlayerPrefs.GetInt("Secondary", 0);
        int _tool = PlayerPrefs.GetInt("Tool", 0);
        
        //Debug.Log("Main: " + _main);

        mainDropdown.value = _main;
        secondaryDropdown.value = _second;
        toolDropdown.value = _tool;
        Debug.Log(_main + ", " + _second + ", " + _tool);
        Debug.Log(mainDropdown.value + ", " + secondaryDropdown.value + ", " + toolDropdown.value);
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
        if(followKiller){
            Debug.Log("followed");
            transform.position = killer.transform.position;
            transform.rotation = killer.transform.rotation;
        }

        healCooldown -= Time.deltaTime;
        if(flashImage.color.a > 0){
            flashImage.color = new Color(1, 1, 1, flashImage.color.a - flashRecovery * Time.deltaTime);
        }
        
    }

    public void spawn(){
        if(!serverEvents.replaying){
            followKiller = false;
            controlsManager.deathMenuControlls = false;
            deathMenu.SetActive(false);
            weaponManager.setWeapons(new List<string> {mainDropdown.options[mainDropdown.value].text, secondaryDropdown.options[secondaryDropdown.value].text, toolDropdown.options[toolDropdown.value].text});
            setClass(GameObject.Find(classDropdown.options[classDropdown.value].text).GetComponent<ClassInfo>());

            look.camRotX = 0;
            coll.enabled = true;
            rb.useGravity = true;
            rb.position = currentMap.spawnPoints[Random.Range(0, currentMap.spawnPoints.Count)].transform.position + Vector3.up;
        }
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

        mainDropdown.ClearOptions();
        secondaryDropdown.ClearOptions();
        toolDropdown.ClearOptions();
        for(int i = 0; i < selectedClass.possibleWeapons.Count; i++){
            if(selectedClass.possibleWeapons[i].main){
                mainDropdown.AddOptions(new List<string> {selectedClass.possibleWeapons[i].gameObject.name});
            }
            if(selectedClass.possibleWeapons[i].secondary){
                secondaryDropdown.AddOptions(new List<string> {selectedClass.possibleWeapons[i].gameObject.name});
            }
            if(selectedClass.possibleWeapons[i].tool){
                toolDropdown.AddOptions(new List<string> {selectedClass.possibleWeapons[i].gameObject.name});
            }
        }
        
        PlayerPrefs.SetInt("Class", classDropdown.value);
        PlayerPrefs.Save();
        //updateWeaponStats(mainDropdown);
    }

    public void updateWeaponStats(TMP_Dropdown dropdown){
        Debug.Log("updated weapon stats");
        WeaponInfo selectedWeapon = weaponManager.weaponContainer.transform.Find(dropdown.options[dropdown.value].text).GetComponent<WeaponInfo>();
        /*ClassInfo selectedClass = GameObject.Find(classDropdown.options[classDropdown.value].text).GetComponent<ClassInfo>();
        for(int i = 0; i < selectedClass.possibleWeapons.Count; i++){
            if(selectedClass.possibleWeapons[i].gameObject.name == mainDropdown.options[mainDropdown.value].text){
                selectedWeapon = selectedClass.possibleWeapons[i];
            }
        }*/

        weaponSpeedText.text = (selectedWeapon.speedMultiplier * 100) + "%";
        weaponDamageText.text = selectedWeapon.damage + "";
        weaponFireRateText.text = selectedWeapon.cooldown + "";
        weaponReloadTimeText.text = selectedWeapon.reloadTime + "";
        weaponClipSizeText.text = selectedWeapon.clipSize + "";
        weaponHeadshotText.text = (selectedWeapon.headShotMult * 100) + "%";
        weaponNameText.text = selectedWeapon.gameObject.name + "";

        PlayerPrefs.SetInt("Main", mainDropdown.value);
        PlayerPrefs.SetInt("Secondary", secondaryDropdown.value);
        PlayerPrefs.SetInt("Tool", toolDropdown.value);
        PlayerPrefs.Save();
    }

    public void commitDie(){
        flashImage.color = new Color(1, 1, 1, 0);
        rb.velocity = Vector3.zero;
        coll.enabled = false;
        rb.useGravity = false;
        deathMenu.SetActive(true);
        rb.position = new Vector3(0f, currentMap.overviewHeight, 0f);
        rb.rotation = Quaternion.identity;
        look.camRotX = 90;
        controlsManager.deathMenuControlls = true;
        weaponManager.changeWeapon(4);
    }

    public List<List<string>> getReplayData(){
        List<List<string>> replayData = new List<List<string>>();
        replayData.Add(replayManager.playerReplayData);
        foreach(OtherPlayer otherClient in serverEvents.clientScripts){
            replayData.Add(otherClient.replayData);
            //Debug.Log(otherClient.gameObject.name);
        }
        return replayData;
    }

    public void death(int killerID){
        if(killerID != serverComm.ID){
            killer = GameObject.Find(killerID + "");
            controlsManager.deathMenuControlls = true;
            weaponManager.changeWeapon(4);
            Debug.Log("DEATHHTTHTHTHTHTHTHTH");
            StartCoroutine(replayManager.startReplay(getReplayData(), killer));
            //followKiller = true;
            flashImage.color = new Color(1, 1, 1, 0);
            look.camRotX = 0;
            rb.velocity = Vector3.zero;
            coll.enabled = false;
            rb.useGravity = false;
        }
        else{
            commitDie();
        }
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
