using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlsManager : MonoBehaviour
{
    public Vector2 moveDirection;
    public Vector2 mouseDelta;

    public PlayerControls playerControls;
    public movement movementScript;
    public look lookScript;
    public bool jumping;
    public bool shooting;
    public bool aiming;
    public bool reloading;
    public bool sprinting;
    public bool crouching;
    public int equippedNum;
    public GunControler gunControler;
    public InGameGUIManager inGameGUIManager;
    public bool escape;

    private void Awake() {
        playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
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
        //I'll change these eventually to a call based system but for now I really don't care enough for the effect frames it would give
        mouseDelta = playerControls.camera.mouseDelta.ReadValue<Vector2>();
        
        moveDirection = playerControls.movement.Walk.ReadValue<Vector2>();
        jumping = playerControls.movement.Jump.ReadValue<float>() == 1;
        sprinting = playerControls.movement.Sprint.ReadValue<float>() == 1;
        crouching = playerControls.movement.Crouch.ReadValue<float>() == 1;

        shooting = playerControls.interactions.shoot.ReadValue<float>() == 1;
        aiming = playerControls.interactions.ADS.ReadValue<float>() == 1;
        reloading = playerControls.interactions.reload.ReadValue<float>() == 1;
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
        if(playerControls.weaponSelects.four.ReadValue<float>() == 1){
            newEquippedNum = 4;
        }
        if(playerControls.weaponSelects.five.ReadValue<float>() == 1){
            newEquippedNum = 5;
        }
        if(playerControls.weaponSelects.six.ReadValue<float>() == 1){
            newEquippedNum = 6;
        }
        if(playerControls.weaponSelects.seven.ReadValue<float>() == 1){
            newEquippedNum = 7;
        }
        if(playerControls.weaponSelects.eight.ReadValue<float>() == 1){
            newEquippedNum = 8;
        }
        if(playerControls.weaponSelects.nine.ReadValue<float>() == 1){
            newEquippedNum = 9;
        }
        if(playerControls.weaponSelects.ten.ReadValue<float>() == 1){
            newEquippedNum = 10;
        }
        if(newEquippedNum != equippedNum){
            gunControler.changeObject(newEquippedNum);
            equippedNum = newEquippedNum;
        }
    }
}
