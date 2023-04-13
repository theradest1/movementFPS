using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Threading.Tasks;

public class WeaponManager : MonoBehaviour
{
    ControlsManager controlsManager;
    movement movementScript;
    SoundManager soundManager;
    public Rigidbody rb;
    public List<WeaponInfo> possibleWeapons;
    public List<WeaponInfo> weapons;
    
    [HideInInspector]
    public WeaponInfo equippedWeapon;

    GameObject cam;
    GameObject player;
    ServerEvents serverEvents;
    public GameObject weaponContainer;
    Look look;
    ProjectileManager projectileManager;
    TextMeshProUGUI objectsInClipText;
    TextMeshProUGUI equippedWeaponText;
    public GunAnimController gunAnimController;

    //public LayerMask playerMask;
    //public LayerMask aimableMask;

    public float weaponRotationSpeed;
    public float weaponTravelSpeed;
    public float reloadingTimer;
    public bool reloading;
    public bool ableToShoot = true;

    public float relaxSpeed;
    public float aimSpeed;
    public float moveRecoverySpeed;
    public Camera gunRenderingCam;
    public Camera mainCam;
    public float FOVChangeSpeed;
    public float gunNormalFOV;
    public float gunScopingFOV;
    public float normalFOV;
    public float scopingFOV;

    public ClassInfo currentClass;
    public float spreadADSMult;
    public float camRecoilPercent;
    public float generalRecoilMult = 1f;

    // Start is called before the first frame update
    void Start()
    {
        look = GameObject.Find("Main Camera").GetComponent<Look>();
        movementScript = GameObject.Find("Player").GetComponent<movement>();
        projectileManager = GameObject.Find("Player").GetComponent<ProjectileManager>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        cam = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
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
        weapons.Add(possibleWeapons[possibleWeapons.Count - 1]);
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

    public bool refillTool(){
        for(int weaponID = 0; weaponID < weapons.Count; weaponID++){
            if(weapons[weaponID].tool){
                if(weapons[weaponID].objectsInClip < (int)(weapons[weaponID].clipSize * currentClass.toolCapacityMult)){
                    Debug.Log(weapons[weaponID].objectsInClip + " < " + (int)(weapons[weaponID].clipSize * currentClass.toolCapacityMult));
                    weapons[weaponID].objectsInClip++;
                    if(equippedWeapon == weapons[weaponID]){
                        objectsInClipText.text = equippedWeapon.objectsInClip + "/" + getModifiedMaxObjects();
                    }
                    return true;
                }
                return false;
            }
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        int modifiedMaxObjects = getModifiedMaxObjects();
        float tempBulletSpeed = equippedWeapon.bulletTravelSpeed;
        if(controlsManager.aiming && equippedWeapon.canADS){
            tempBulletSpeed *= equippedWeapon.bulletSpeedADSMult;
        }
        
        if(!controlsManager.shooting){
            ableToShoot = true;
        }
        
        //weaponContainer.transform.rotation = Quaternion.Slerp(weaponContainer.transform.rotation, player.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        //equippedWeapon.transform.rotation = Quaternion.Slerp(equippedWeapon.transform.rotation, cam.transform.rotation, weaponRotationSpeed * Time.deltaTime);
        
        if((controlsManager.reloading && !reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip < modifiedMaxObjects) || (controlsManager.shooting && !reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip <= 0)){
            reloading = true;
            reloadingTimer = gunAnimController.triggerReload(currentClass.reloadSpeedMult);//equippedWeapon.reloadTime;
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
            
            // cam.transform.Rotate(Random.Range(-equippedWeapon.recoilVertical, 0), Random.Range(-equippedWeapon.recoilHorizontal, equippedWeapon.recoilHorizontal), 0f);
            reloading = false;
            equippedWeapon.objectsInClip -= 1;
            objectsInClipText.text = equippedWeapon.objectsInClip + "/" + modifiedMaxObjects;
            equippedWeapon.cooldownTimer = gunAnimController.triggerShoot(currentClass.gunFireSpeedMult);//equippedWeapon.cooldown;

            if(equippedWeapon.projectileID == 3){ //only for bullets
                projectileManager.createProjectile(0, 0, equippedWeapon.damage, cam.transform.position, cam.transform.forward * tempBulletSpeed + rb.velocity);
            }
            if(equippedWeapon.projectileID == 6){ //only for shotgunShells
                projectileManager.createProjectile(0, 5, equippedWeapon.damage, cam.transform.position, cam.transform.forward * tempBulletSpeed + rb.velocity);
            }

            serverEvents.sendEvent("ue", "pr", equippedWeapon.projectileID + "~" + equippedWeapon.damage + "~" + cam.transform.position + "~" + (cam.transform.forward * tempBulletSpeed + rb.velocity) + "~" + equippedWeapon.shootSound + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
            soundManager.playSound(equippedWeapon.shootSound, cam.transform.position, equippedWeapon.shootVolume, equippedWeapon.shootPitch, cam.transform);
            //serverEvents.sendEvent("ue", "sound",  + "~" + equippedWeapon.transform.position + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
            //look.camRotX
            
            
            equippedWeapon.transform.localEulerAngles = equippedWeapon.transform.localEulerAngles - new Vector3(Random.Range(equippedWeapon.recoilVerticalMin * spreadADSMult * generalRecoilMult, equippedWeapon.recoilVerticalMax * spreadADSMult * generalRecoilMult), 0f, 0f); 
            equippedWeapon.transform.Rotate(0f, Random.Range(-equippedWeapon.recoilHorizontal * spreadADSMult * generalRecoilMult, equippedWeapon.recoilHorizontal * spreadADSMult * generalRecoilMult), 0f);
            look.camRotX -= Random.Range(equippedWeapon.recoilVerticalMin * spreadADSMult * generalRecoilMult, equippedWeapon.recoilVerticalMax * spreadADSMult * generalRecoilMult) * camRecoilPercent;
        }

        if(controlsManager.aiming && equippedWeapon.canADS){
            equippedWeapon.transform.localPosition = Vector3.Lerp(equippedWeapon.transform.localPosition, equippedWeapon.scopingPos, aimSpeed * Time.deltaTime);
              //equippedWeapon.transform.localRotation = Quaternion.Euler(Vector3.Slerp(equippedWeapon.transform.localEulerAngles, equippedWeapon.scopingRot, relaxSpeed * Time.deltaTime));
            //equippedWeapon.transform.localRotation = Quaternion.Euler(equippedWeapon.scopingRot);
            equippedWeapon.transform.localRotation = Quaternion.Slerp(equippedWeapon.transform.localRotation, equippedWeapon.scopingRotQ, relaxSpeed * Time.deltaTime);
              //equippedWeapon.transform.localEulerAngles = new Vector3(Mathf.Lerp(equippedWeapon.transform.localEulerAngles.x, equippedWeapon.scopingRot.x, relaxSpeed * Time.deltaTime), equippedWeapon.transform.localEulerAngles.y, equippedWeapon.transform.localEulerAngles.z);
            gunRenderingCam.fieldOfView = Mathf.Lerp(gunRenderingCam.fieldOfView, gunScopingFOV, FOVChangeSpeed * Time.deltaTime);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, scopingFOV, FOVChangeSpeed * Time.deltaTime);
        }
        else{
            equippedWeapon.transform.localPosition = Vector3.Lerp(equippedWeapon.transform.localPosition, equippedWeapon.restingPos, relaxSpeed * Time.deltaTime);
              //equippedWeapon.transform.localRotation = Quaternion.Euler(Vector3.Slerp(equippedWeapon.transform.localEulerAngles, equippedWeapon.restingRot, relaxSpeed * Time.deltaTime));
            //equippedWeapon.transform.localRotation = Quaternion.Euler(equippedWeapon.restingRot);
              //equippedWeapon.transform.localEulerAngles = new Vector3(Mathf.Lerp(equippedWeapon.transform.localEulerAngles.x, equippedWeapon.restingRot.x, relaxSpeed * Time.deltaTime), equippedWeapon.transform.localEulerAngles.y, equippedWeapon.transform.localEulerAngles.z);
            equippedWeapon.transform.localRotation = Quaternion.Slerp(equippedWeapon.transform.localRotation, equippedWeapon.scopingRotQ, relaxSpeed * Time.deltaTime);
            mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, normalFOV, FOVChangeSpeed * Time.deltaTime);
            gunRenderingCam.fieldOfView = Mathf.Lerp(gunRenderingCam.fieldOfView, gunNormalFOV, FOVChangeSpeed * Time.deltaTime);
        }
        weaponContainer.transform.localPosition = Vector3.Lerp(weaponContainer.transform.localPosition, Vector3.zero, moveRecoverySpeed * Time.deltaTime);
    }
}
