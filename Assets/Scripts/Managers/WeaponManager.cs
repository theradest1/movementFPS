using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class WeaponManager : MonoBehaviour
{
    ControlsManager controlsManager;
    movement movementScript;
    public List<WeaponInfo> possibleWeapons;
    public List<WeaponInfo> weapons;
    
    [HideInInspector]
    public WeaponInfo equippedWeapon;

    GameObject cam;
    GameObject player;
    ServerEvents serverEvents;
    public GameObject weaponContainer;
    ProjectileManager projectileManager;
    SoundManager soundManager;
    TextMeshProUGUI objectsInClipText;
    TextMeshProUGUI equippedWeaponText;

    //public LayerMask playerMask;
    //public LayerMask aimableMask;

    public float weaponRotationSpeed;
    public float weaponTravelSpeed;
    public float reloadingTimer;
    public bool reloading;
    public bool ableToShoot = true;

    public ClassInfo currentClass;

    // Start is called before the first frame update
    void Start()
    {
        movementScript = GameObject.Find("Player").GetComponent<movement>();
        projectileManager = GameObject.Find("Player").GetComponent<ProjectileManager>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        cam = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        equippedWeaponText = GameObject.Find("equipped weapon").GetComponent<TextMeshProUGUI>();
        objectsInClipText = GameObject.Find("ammo left").GetComponent<TextMeshProUGUI>();

        changeWeapon(1);
    }

    public void setWeapons(List<string> equippedWeapons){
        weapons = new List<WeaponInfo>();
        for(int i = 0; i < possibleWeapons.Count; i++){
            if(equippedWeapons.Contains(possibleWeapons[i].gameObject.name)){
                //Debug.Log("using " + possibleWeapons[i].gameObject.name);
                weapons.Add(possibleWeapons[i]);
            }
        }
        changeWeapon(1);
    } 

    public void resetAllWeapons(){
        for(int weaponID = 0; weaponID < weapons.Count; weaponID++){
            equippedWeapon = weapons[weaponID];
            equippedWeapon.objectsInClip = getModifiedMaxObjects();//(int)(weapons[weaponID].clipSize * currentClass.ammoCapacityMult);
            equippedWeapon.cooldownTimer = 0f;
            equippedWeapon.gameObject.SetActive(false);
        }
        equippedWeapon = weapons[0];
        controlsManager.equippedNum = 1;
        equippedWeapon.gameObject.SetActive(true);
        objectsInClipText.text = equippedWeapon.objectsInClip + "/" + getModifiedMaxObjects();
    }

    public void changeWeapon(int newWeapon){
        if(newWeapon <= weapons.Count){
            if(equippedWeapon != null){
                equippedWeapon.gameObject.SetActive(false);
            }
            reloading = false;
            ableToShoot = true;
            
            equippedWeapon = weapons[newWeapon - 1];
            equippedWeapon.gameObject.SetActive(true);
            equippedWeapon.transform.localRotation = Quaternion.Euler(50f, equippedWeapon.transform.localEulerAngles.y, equippedWeapon.transform.localEulerAngles.z); 
            
            if(equippedWeapon.cooldownTimer < equippedWeapon.equipCooldown){
                equippedWeapon.cooldownTimer = equippedWeapon.equipCooldown;
            }
            equippedWeaponText.text = equippedWeapon.gameObject.name;
            movementScript.speedMultiplierFromWeapon = equippedWeapon.speedMultiplier;
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + getModifiedMaxObjects();
        }
    }

    int getModifiedMaxObjects(){
        if(equippedWeapon.gun){
            return (int)(equippedWeapon.clipSize * currentClass.ammoCapacityMult);
        }
        else if(equippedWeapon.tool){
            return (int)(equippedWeapon.clipSize * currentClass.toolCapacityMult);
        }
        return 0;
    }

    // Update is called once per frame
    void Update()
    {
        int modifiedMaxObjects = getModifiedMaxObjects();

        
        if(!controlsManager.shooting){
            ableToShoot = true;
        }
        
        weaponContainer.transform.rotation = Quaternion.Slerp(weaponContainer.transform.rotation, player.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        equippedWeapon.transform.rotation = Quaternion.Slerp(equippedWeapon.transform.rotation, cam.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        if((controlsManager.reloading && !reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip < modifiedMaxObjects) || (controlsManager.shooting && !reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip <= 0)){
            reloading = true;
            reloadingTimer = equippedWeapon.reloadTime * currentClass.reloadSpeedMult;
            objectsInClipText.text = "--/" + modifiedMaxObjects;
            serverEvents.sendEvent("ue", "sound", equippedWeapon.reloadSound + "~" + equippedWeapon.transform.position + "~1~1");
        }

        reloadingTimer -= Time.deltaTime;
        if(reloading && reloadingTimer <= 0 && equippedWeapon.objectsInClip < modifiedMaxObjects){
            reloading = false;
            equippedWeapon.objectsInClip = modifiedMaxObjects;
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + modifiedMaxObjects;
        }

        if(controlsManager.shooting && equippedWeapon.objectsInClip > 0 && equippedWeapon.cooldownTimer <= 0 && ableToShoot){
            if(!equippedWeapon.automatic){
                ableToShoot = false;
            }
            reloading = false;
            equippedWeapon.objectsInClip -= 1;
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + modifiedMaxObjects;
            equippedWeapon.cooldownTimer = equippedWeapon.cooldown * currentClass.gunFireSpeedMult;
        
            if(equippedWeapon.projectileID == 3){ //only for bullets
                projectileManager.createProjectile(0, 0, equippedWeapon.damage, cam.transform.position, cam.transform.forward * equippedWeapon.bulletTravelSpeed);
            }
            if(equippedWeapon.projectileID == 6){ //only for shotgunShells
                projectileManager.createProjectile(0, 5, equippedWeapon.damage, cam.transform.position, cam.transform.forward * equippedWeapon.bulletTravelSpeed);
            }

            serverEvents.sendEvent("ue", "pr", equippedWeapon.projectileID + "~" + equippedWeapon.damage + "~" + cam.transform.position + "~" + cam.transform.forward * equippedWeapon.bulletTravelSpeed + "~" + equippedWeapon.shootSound + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
            //serverEvents.sendEvent("ue", "sound",  + "~" + equippedWeapon.transform.position + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
        }
    }
}
