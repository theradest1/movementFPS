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
    public UsableTools tools; //grapple, air control, heal
    public Abilities abilities; //dash, heal, teleport

    public TMP_Dropdown weaponDropdown;
	public TMP_Dropdown toolDropdown;
	public TMP_Dropdown abilityDropdown;
	public TMP_Dropdown throwableDropdown;
    
    public Slider weaponCooldown;
	public Slider toolCooldown;
	public Slider abilityCooldown;
	public Slider throwableCooldown;

    public void resetAllObjets(){
        weapons.resetAll();
        throwables.resetAll();
        tools.resetAll();
        abilities.resetAll();
    }

    public void updateInventory(){
        throwables.setThrowable(throwableDropdown.value);
        abilities.setAbility(abilityDropdown.value);
    }

    void Update() {
        weaponCooldown.value = weapons.getCooldownPercentage();

        toolCooldown.value = 0f; //not added yet

        abilityCooldown.value = abilities.getCooldownPercentage();
        throwableCooldown.value = throwables.cooldownTimer/throwables.equippedThrowable.cooldown;
    }
}
