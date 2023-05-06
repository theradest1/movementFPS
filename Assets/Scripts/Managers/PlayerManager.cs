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
	public TMP_Dropdown mainDropdown;
	public TMP_Dropdown toolDropdown;
	public GameObject killer;
	bool followKiller = false;
	Collider coll;
	Rigidbody rb;
	public MapInfo currentMap;

	public TextMeshProUGUI weaponSpeedText;
	public TextMeshProUGUI weaponDamageText;
	public TextMeshProUGUI weaponFireRateText;
	public TextMeshProUGUI weaponReloadTimeText;
	public TextMeshProUGUI weaponClipSizeText;
	public TextMeshProUGUI weaponHeadshotText;
	public TextMeshProUGUI weaponNameText;

	void Start()
	{
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
		changeHealth(0f);

		Invoke("getWeaponChoices", .1f);
	}

	void getWeaponChoices()
	{
		int _main = PlayerPrefs.GetInt("Main", 0);
		int _tool = PlayerPrefs.GetInt("Tool", 0);

		//Debug.Log("Main: " + _main);

		mainDropdown.value = _main;
		toolDropdown.value = _tool;
		//Debug.Log(_main + ", " + _second + ", " + _tool);
		//Debug.Log(mainDropdown.value + ", " + toolDropdown.value);
	}

	private void Update()
	{
		if (followKiller)
		{
			Debug.Log("followed");
			transform.position = killer.transform.position;
			transform.rotation = killer.transform.rotation;
		}

		if (flashImage.color.a > 0)
		{
			flashImage.color = new Color(1, 1, 1, flashImage.color.a - flashRecovery * Time.deltaTime);
		}

	}

	public void spawn()
	{
		if (!serverEvents.replaying)
		{
			followKiller = false;
			controlsManager.deathMenuControlls = false;
			deathMenu.SetActive(false);
			weaponManager.setWeapons(new List<string> { mainDropdown.options[mainDropdown.value].text, null, toolDropdown.options[toolDropdown.value].text });

			look.camRotX = 0;
			coll.enabled = true;
			rb.useGravity = true;
			rb.position = currentMap.spawnPoints[Random.Range(0, currentMap.spawnPoints.Count)].transform.position + Vector3.up;
		}
	}

	public void updateWeaponStats(TMP_Dropdown dropdown)
	{
		Debug.Log("updated weapon stats");
		WeaponInfo selectedWeapon = weaponManager.weaponContainer.transform.Find(dropdown.options[dropdown.value].text).GetComponent<WeaponInfo>();

		weaponSpeedText.text = (selectedWeapon.speedMultiplier * 100) + "%";
		weaponDamageText.text = selectedWeapon.damage + "";
		weaponFireRateText.text = selectedWeapon.cooldown + "";
		weaponReloadTimeText.text = selectedWeapon.reloadTime + "";
		weaponClipSizeText.text = selectedWeapon.clipSize + "";
		weaponHeadshotText.text = (selectedWeapon.headShotMult * 100) + "%";
		weaponNameText.text = selectedWeapon.gameObject.name + "";

		PlayerPrefs.SetInt("Main", mainDropdown.value);
		PlayerPrefs.SetInt("Tool", toolDropdown.value);
		PlayerPrefs.Save();
	}

	public void commitDie()
	{
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

	public List<List<string>> getReplayData()
	{
		List<List<string>> replayData = new List<List<string>>();
		replayData.Add(replayManager.playerReplayData);
		foreach (OtherPlayer otherClient in serverEvents.clientScripts)
		{
			replayData.Add(otherClient.replayData);
			//Debug.Log(otherClient.gameObject.name);
		}
		return replayData;
	}

	public void death(int killerID)
	{
		if (killerID != serverComm.ID)
		{
			killer = GameObject.Find(killerID + "");
			controlsManager.deathMenuControlls = true;
			weaponManager.changeWeapon(4);
			//Debug.Log("DEATHHTTHTHTHTHTHTHTH");
			StartCoroutine(replayManager.startReplay(getReplayData(), killer));
			//followKiller = true;
			flashImage.color = new Color(1, 1, 1, 0);
			look.camRotX = 0;
			rb.velocity = Vector3.zero;
			coll.enabled = false;
			rb.useGravity = false;
		}
		else
		{
			commitDie();
		}
	}

	public void changeHealth(float subbedHealth)
	{
		health = Mathf.Clamp(health - subbedHealth, 0f, maxHealth);
		healthSlider.value = health / maxHealth;
		healthText.text = Mathf.Round(health) + "/" + maxHealth;
	}
}
