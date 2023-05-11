using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Weapons weapons; //guns, swords
    public Throwables throwables; //granade, flash, smoke, molley
    public UsableTools tools; //grapple, air control, heal
    public Abilities abilities; //dash, heal, teleport
    
    [Header("References:")]
    public ControlsManager controlsManager;

    public void resetAllObjets(){
        weapons.resetAll();
        throwables.resetAll();
        tools.resetAll();
        abilities.resetAll();
    }
}
