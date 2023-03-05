using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    [HideInInspector]
    public Vector2 moveDirection;
    [HideInInspector]
    public Vector2 mouseDelta;

    public PlayerControls playerControls;
    movement movementScript;
    Look look;
    WeaponManager weaponManager;
    InGameGUIManager inGameGUIManager;

    [HideInInspector]
    public bool jumping;
    [HideInInspector]
    public bool shooting;
    [HideInInspector]
    public bool aiming;
    [HideInInspector]
    public bool reloading;
    [HideInInspector]
    public bool sprinting;
    [HideInInspector]
    public bool crouching;
    [HideInInspector]
    public bool escape;
    [HideInInspector]
    public int equippedNum;
    [HideInInspector]
    public bool tab;
    public bool deathMenuControlls = true;

    private void Start() {
        movementScript = GameObject.Find("Player").GetComponent<movement>();
        look = GameObject.Find("Main Camera").GetComponent<Look>();
        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
        inGameGUIManager = GameObject.Find("manager").GetComponent<InGameGUIManager>();
    }
    

    private void Awake() {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Update()
    {
        if(!deathMenuControlls){
            //I'll change these eventually to a call based system but for now I really don't care enough for the effect frames it would give
            mouseDelta = playerControls.camera.mouseDelta.ReadValue<Vector2>();
            
            moveDirection = playerControls.movement.Walk.ReadValue<Vector2>();
            jumping = playerControls.movement.Jump.ReadValue<float>() == 1;
            sprinting = playerControls.movement.Sprint.ReadValue<float>() == 1;
            crouching = playerControls.movement.Crouch.ReadValue<float>() == 1;

            shooting = playerControls.interactions.shoot.ReadValue<float>() == 1;
            aiming = playerControls.interactions.ADS.ReadValue<float>() == 1;
            reloading = playerControls.interactions.reload.ReadValue<float>() == 1;

            tab = playerControls.interactions.tab.ReadValue<float>() == 1;

            if(escape != (playerControls.interactions.escape.ReadValue<float>() == 1) && escape == false){
                inGameGUIManager.changeGUIState();
            }
            escape = playerControls.interactions.escape.ReadValue<float>() == 1;

            int newEquippedNum = equippedNum;
            if(playerControls.weaponSelects.one.ReadValue<float>() == 1){
                newEquippedNum = 1;
            }
            if(playerControls.weaponSelects.two.ReadValue<float>() == 1){
                newEquippedNum = 2;
            }
            if(playerControls.weaponSelects.three.ReadValue<float>() == 1){
                newEquippedNum = 3;
            }
            newEquippedNum = Mathf.Clamp(newEquippedNum + (int)Mathf.Clamp(playerControls.weaponSelects.scroll.ReadValue<Vector2>().y, -1, 1), 1, 3);
            if(newEquippedNum != equippedNum){
                weaponManager.changeWeapon(newEquippedNum);
                equippedNum = newEquippedNum;
            }

        }
        else{
            
            mouseDelta = Vector2.zero;
            moveDirection = Vector3.zero;
            jumping = false;
            sprinting = false;
            crouching = false;
            shooting = false;
            aiming = false;
            reloading = false;

            tab = playerControls.interactions.tab.ReadValue<float>() == 1;
            if(escape != (playerControls.interactions.escape.ReadValue<float>() == 1) && escape == false){
                inGameGUIManager.changeGUIState();
            }
            escape = playerControls.interactions.escape.ReadValue<float>() == 1;
        }
    }
}
