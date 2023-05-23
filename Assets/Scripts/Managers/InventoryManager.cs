using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [Header("References:")]
    public Weapons weapons; //guns, swords
    public Throwables throwables; //granade, flash, smoke, molley
    public Tools tools; //grapple, air control, heal
    public Abilities abilities; //dash, heal, teleport

    public TMP_Dropdown weaponDropdown;
	public TMP_Dropdown toolDropdown;
	public TMP_Dropdown abilityDropdown;
	public TMP_Dropdown throwableDropdown;
    
    public Slider weaponCooldown;
	public Slider toolCooldown;
	public Slider abilityCooldown;
	public Slider throwableCooldown;

    public TextMeshProUGUI toolCharges;
    public TextMeshProUGUI throwableCharges;

    public void resetAllObjets(){
        weapons.resetAll();
        throwables.resetAll();
        tools.resetAll();
        abilities.resetAll();
    }

    private void Start()
    {
        Invoke("loadInventory", .1f);
    }

    public void loadInventory(){
        int _weapon = PlayerPrefs.GetInt("Weapon", 0);
		int _tool = PlayerPrefs.GetInt("Tool", 0);
		int _ability = PlayerPrefs.GetInt("Ability", 0);
		int _throwable = PlayerPrefs.GetInt("Throwable", 0);

		//Debug.Log("Main: " + _main);

		weaponDropdown.value = _weapon;
		toolDropdown.value = _tool;
		abilityDropdown.value = _ability;
		throwableDropdown.value = _throwable;
    }

    /*public void updateWeaponStats(TMP_Dropdown dropdown)
	{
		Debug.Log("updated weapon stats");
		WeaponInfo selectedWeapon = weapons.weaponContainer.transform.Find(dropdown.options[dropdown.value].text).GetComponent<WeaponInfo>();

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
	}*/

    public void updateInventory(){
        throwables.setThrowable(throwableDropdown.value);
        abilities.setAbility(abilityDropdown.value);
        tools.setTool(toolDropdown.value);

        PlayerPrefs.SetInt("Weapon", weaponDropdown.value);
		PlayerPrefs.SetInt("Tool", toolDropdown.value);
		PlayerPrefs.SetInt("Ability", abilityDropdown.value);
		PlayerPrefs.SetInt("Throwable", throwableDropdown.value);
        PlayerPrefs.Save();
    }

    void Update() {
        weaponCooldown.value = weapons.getCooldownPercentage();

        toolCooldown.value = tools.getCooldownPercentage();

        abilityCooldown.value = abilities.getCooldownPercentage();
        throwableCooldown.value = throwables.cooldownTimer/throwables.equippedThrowable.cooldown;

        toolCharges.text = tools.charges + "";
        throwableCharges.text = throwables.throwableCharges + "";
    }
}
