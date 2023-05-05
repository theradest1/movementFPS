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
	Grapple grapple;
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
	GunAnimController currentGunAnimController;
	//public GunAnimController gunAnimController;

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


	public float spreadADSMult;
	public float camRecoilPercent;
	public float generalRecoilMult = 1f;
	public float casingLifetime = 3;

	// Start is called before the first frame update
	void Start()
	{
		grapple = GameObject.Find("Player").GetComponent<Grapple>();
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

	public void setWeapons(List<string> equippedWeapons)
	{
		weapons = new List<WeaponInfo>();
		for (int i = 0; i < possibleWeapons.Count; i++)
		{
			if (equippedWeapons.Contains(possibleWeapons[i].gameObject.name))
			{
				//Debug.Log("using " + possibleWeapons[i].gameObject.name);
				weapons.Add(possibleWeapons[i]);
			}
		}
		weapons.Add(possibleWeapons[possibleWeapons.Count - 1]);
		changeWeapon(1);
	}

	public void resetAllWeapons()
	{
		for (int weaponID = 0; weaponID < weapons.Count; weaponID++)
		{
			equippedWeapon = weapons[weaponID];
			equippedWeapon.objectsInClip = getModifiedMaxObjects();//(int)(weapons[weaponID].clipSize);
			equippedWeapon.cooldownTimer = 0f;
			equippedWeapon.gameObject.SetActive(false);
		}
		equippedWeapon = weapons[0];
		controlsManager.equippedNum = 1;
		equippedWeapon.gameObject.SetActive(true);
		objectsInClipText.text = equippedWeapon.objectsInClip + "/" + getModifiedMaxObjects();
	}

	public void changeWeapon(int newWeapon)
	{
		if (newWeapon <= weapons.Count)
		{
			if (equippedWeapon != null)
			{
				equippedWeapon.gameObject.SetActive(false);
			}
			reloading = false;
			ableToShoot = true;

			equippedWeapon = weapons[newWeapon - 1];
			equippedWeapon.gameObject.SetActive(true);
			equippedWeapon.transform.localRotation = Quaternion.Euler(50f, equippedWeapon.transform.localEulerAngles.y, equippedWeapon.transform.localEulerAngles.z);

			if (equippedWeapon.cooldownTimer < equippedWeapon.equipCooldown)
			{
				equippedWeapon.cooldownTimer = equippedWeapon.equipCooldown;
			}
			equippedWeaponText.text = equippedWeapon.gameObject.name;
			movementScript.speedMultiplierFromWeapon = equippedWeapon.speedMultiplier;
			objectsInClipText.text = equippedWeapon.objectsInClip + "/" + getModifiedMaxObjects();
			currentGunAnimController = equippedWeapon.GetComponent<GunAnimController>();
			//Debug.Log("set");
		}
	}

	int getModifiedMaxObjects()
	{
		if (equippedWeapon.gun)
		{
			return (int)(equippedWeapon.clipSize);
		}
		else if (equippedWeapon.tool)
		{
			return (int)(equippedWeapon.clipSize);
		}
		return 0;
	}

	public bool refillTool()
	{
		for (int weaponID = 0; weaponID < weapons.Count; weaponID++)
		{
			if (weapons[weaponID].tool)
			{
				if (weapons[weaponID].objectsInClip < (int)(weapons[weaponID].clipSize))
				{
					//Debug.Log(weapons[weaponID].objectsInClip + " < " + (int)(weapons[weaponID].clipSize));
					weapons[weaponID].objectsInClip++;
					if (equippedWeapon == weapons[weaponID])
					{
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
		if (controlsManager.aiming && equippedWeapon.canADS)
		{
			tempBulletSpeed *= equippedWeapon.bulletSpeedADSMult;
		}
		if (!controlsManager.shooting)
		{
			ableToShoot = true;
		}

		//weaponContainer.transform.rotation = Quaternion.Slerp(weaponContainer.transform.rotation, player.transform.rotation, weaponRotationSpeed * Time.deltaTime);

		//equippedWeapon.transform.rotation = Quaternion.Slerp(equippedWeapon.transform.rotation, cam.transform.rotation, weaponRotationSpeed * Time.deltaTime);


		// start reloading
		if ((controlsManager.reloading && equippedWeapon.reloadable && equippedWeapon.objectsInClip < modifiedMaxObjects) || (controlsManager.shooting && equippedWeapon.reloadable && equippedWeapon.objectsInClip <= 0))
		{
			if (!reloading)
			{
				reloading = true;
				reloadingTimer = equippedWeapon.reloadTime;
				if (equippedWeapon.individualBulletReload)
				{
					reloadingTimer -= reloadingTimer * (equippedWeapon.objectsInClip - 1) / modifiedMaxObjects + .01f;
				}
				currentGunAnimController.triggerReload(equippedWeapon.reloadTime);
				if (equippedWeapon.unscopeAfterReload)
				{
					controlsManager.aiming = false;
				}

				if (!equippedWeapon.individualBulletReload)
				{
					objectsInClipText.text = "--/" + modifiedMaxObjects;
				}

				serverEvents.sendEvent("ue", "sound", equippedWeapon.startReloadSoundID + "~" + equippedWeapon.transform.position + "~1~1");
			}
		}

		//per-bullet reloading
		if(reloading && reloadingTimer >= 0 && equippedWeapon.individualBulletReload){
			if(equippedWeapon.objectsInClip != modifiedMaxObjects - (int)(reloadingTimer / (equippedWeapon.reloadTime / (float)modifiedMaxObjects))){
				equippedWeapon.objectsInClip = modifiedMaxObjects - (int)(reloadingTimer / (equippedWeapon.reloadTime / (float)modifiedMaxObjects));
				if((int)(reloadingTimer / (equippedWeapon.reloadTime / (float)modifiedMaxObjects)) == 0){
					soundManager.playSound(equippedWeapon.endReloadSoundID, cam.transform.position, 1f, 1f, cam.transform);
				}
				else{
					soundManager.playSound(equippedWeapon.reloadPerBulletSoundID, cam.transform.position, 1f, 1f, cam.transform);
				}
			}
			objectsInClipText.text = equippedWeapon.objectsInClip + "/" + modifiedMaxObjects;
			if((int)(reloadingTimer / (equippedWeapon.reloadTime / (float)modifiedMaxObjects)) <= 0){
				currentGunAnimController.animator.SetBool("reloading", false);
			}
			else{
				currentGunAnimController.animator.SetBool("reloading", true);
			}
		}
		else if(reloading && reloadingTimer >= 0 && !equippedWeapon.individualBulletReload){
            currentGunAnimController.animator.SetBool("reloading", true);
		}
        else{
            currentGunAnimController.animator.SetBool("reloading", false);
        }

		// reloading
		reloadingTimer -= Time.deltaTime;
		if (reloading && reloadingTimer <= 0 && equippedWeapon.objectsInClip < modifiedMaxObjects)
		{
			reloading = false;
			equippedWeapon.objectsInClip = modifiedMaxObjects;
			objectsInClipText.text = equippedWeapon.objectsInClip + "/" + modifiedMaxObjects;
			soundManager.playSound(equippedWeapon.endReloadSoundID, cam.transform.position, 1f, 1f, cam.transform);
			Debug.Log("finished reloading");
		}

		//shooting
		if (controlsManager.shooting && equippedWeapon.objectsInClip > 0 && equippedWeapon.cooldownTimer <= 0 && ableToShoot)
		{
			if (!equippedWeapon.automatic)
			{
				ableToShoot = false;
			}

			// cam.transform.Rotate(Random.Range(-equippedWeapon.recoilVertical, 0), Random.Range(-equippedWeapon.recoilHorizontal, equippedWeapon.recoilHorizontal), 0f);
			reloading = false;
			equippedWeapon.objectsInClip -= 1;
			objectsInClipText.text = equippedWeapon.objectsInClip + "/" + modifiedMaxObjects;
			equippedWeapon.cooldownTimer = equippedWeapon.cooldown;
			equippedWeapon.GetComponent<GunAnimController>().triggerShoot(equippedWeapon.cooldown);

			if (equippedWeapon.projectileID == 3)
			{ //only for bullets
				projectileManager.createProjectile(0, 0, equippedWeapon.damage, cam.transform.position, cam.transform.forward * tempBulletSpeed + rb.velocity);
			}
			if (equippedWeapon.projectileID == 6)
			{ //only for shotgunShells
				projectileManager.createProjectile(0, 5, equippedWeapon.damage, cam.transform.position, cam.transform.forward * tempBulletSpeed + rb.velocity);
			}

			serverEvents.sendEvent("ue", "pr", equippedWeapon.projectileID + "~" + equippedWeapon.damage + "~" + cam.transform.position + "~" + (cam.transform.forward * tempBulletSpeed + rb.velocity) + "~" + equippedWeapon.shootSound + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
			soundManager.playSound(equippedWeapon.shootSound, cam.transform.position, equippedWeapon.shootVolume, equippedWeapon.shootPitch, cam.transform);
			//serverEvents.sendEvent("ue", "sound",  + "~" + equippedWeapon.transform.position + "~" + equippedWeapon.shootVolume + "~" + equippedWeapon.shootPitch);
			//look.camRotX


			equippedWeapon.transform.localEulerAngles = equippedWeapon.transform.localEulerAngles - new Vector3(Random.Range(equippedWeapon.recoilVerticalMin * spreadADSMult * generalRecoilMult, equippedWeapon.recoilVerticalMax * spreadADSMult * generalRecoilMult), 0f, 0f);
			equippedWeapon.transform.Rotate(0f, Random.Range(-equippedWeapon.recoilHorizontal * spreadADSMult * generalRecoilMult, equippedWeapon.recoilHorizontal * spreadADSMult * generalRecoilMult), 0f);
			look.camRotX -= Random.Range(equippedWeapon.recoilVerticalMin * spreadADSMult * generalRecoilMult, equippedWeapon.recoilVerticalMax * spreadADSMult * generalRecoilMult) * camRecoilPercent;

			if (equippedWeapon.unscopeAfterShoot)
			{
				controlsManager.aiming = false;
			}

			if(equippedWeapon.ejectCasings){
				GameObject casing = Instantiate(equippedWeapon.casingPrefab, equippedWeapon.casingStartPos.position, equippedWeapon.casingStartPos.rotation);
				casing.GetComponent<Rigidbody>().AddRelativeForce(equippedWeapon.casingVelocity);
				casing.GetComponent<Rigidbody>().velocity += rb.velocity;
				Destroy(casing, casingLifetime);
			}
		}


		// aiming down sights
		if (controlsManager.aiming && equippedWeapon.canADS && !reloading)
		{
			equippedWeapon.transform.localPosition = Vector3.Lerp(equippedWeapon.transform.localPosition, equippedWeapon.scopingPos, aimSpeed * Time.deltaTime);
			//equippedWeapon.transform.localRotation = Quaternion.Euler(Vector3.Slerp(equippedWeapon.transform.localEulerAngles, equippedWeapon.scopingRot, relaxSpeed * Time.deltaTime));
			//equippedWeapon.transform.localRotation = Quaternion.Euler(equippedWeapon.scopingRot);
			equippedWeapon.transform.localRotation = Quaternion.Slerp(equippedWeapon.transform.localRotation, equippedWeapon.scopingRotQ, relaxSpeed * Time.deltaTime);
			//equippedWeapon.transform.localEulerAngles = new Vector3(Mathf.Lerp(equippedWeapon.transform.localEulerAngles.x, equippedWeapon.scopingRot.x, relaxSpeed * Time.deltaTime), equippedWeapon.transform.localEulerAngles.y, equippedWeapon.transform.localEulerAngles.z);
			gunRenderingCam.fieldOfView = Mathf.Lerp(gunRenderingCam.fieldOfView, gunScopingFOV, FOVChangeSpeed * Time.deltaTime);
			mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, scopingFOV, FOVChangeSpeed * Time.deltaTime);
		}
		else
		{
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
