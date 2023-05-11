using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    movement movementScript;
    Look look;
    WeaponManager weaponManager;
    InGameGUIManager inGameGUIManager;
    ReplayManager replayManager;

    [Header("References:")]
    public PlayerControls playerControls;

    //mouse and movement
    [HideInInspector]
    public Vector2 moveDirection;
    [HideInInspector]
    public Vector2 mouseDelta;

    //weapon
    [HideInInspector]
    public bool aiming;
    [HideInInspector]
    public bool reloading;

    //inventory use
    [HideInInspector]
    public bool weaponUse;
    [HideInInspector]
    public bool throwableUse;
    [HideInInspector]
    public bool abilityUse;
    [HideInInspector]
    public bool toolUse;

    //movement
    [HideInInspector]
    public bool jumping;

    //menu
    [HideInInspector]
    public bool escape;
    [HideInInspector]
    public bool openScoreBoard;
    [HideInInspector]
    public bool chat = false;

    //events
    [HideInInspector]
    public bool disconnected = false;
    [HideInInspector]
    public bool inMenu = false;
    [HideInInspector]
    public bool deathMenuControlls = true;
    [HideInInspector]
    public bool choosingMap = false;
    [HideInInspector]
    public bool chatting = false;

    private void Start() {
        replayManager = GameObject.Find("manager").GetComponent<ReplayManager>();
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
        if(!deathMenuControlls && !disconnected && !inMenu && !choosingMap && !chatting){
            //I'll change these eventually to a call based system but for now I really don't care enough for the effect frames it would give
            //basic things
            Cursor.lockState = CursorLockMode.Locked;
            mouseDelta = playerControls.camera.mouseDelta.ReadValue<Vector2>();
            moveDirection = playerControls.movement.moveDirection.ReadValue<Vector2>();
            
            //global movement
            jumping = playerControls.movement.jump.ReadValue<float>() == 1;

            //inventory use
            weaponUse = playerControls.interactions.weaponUse.ReadValue<float>() == 1;
            throwableUse = playerControls.interactions.throwableUse.ReadValue<float>() == 1;
            abilityUse = playerControls.interactions.abilityUse.ReadValue<float>() == 1;
            toolUse = playerControls.interactions.toolUse.ReadValue<float>() == 1;

            //weapon
            reloading = playerControls.interactions.reload.ReadValue<float>() == 1;
            aiming = playerControls.interactions.aiming.ReadValue<float>() == 1;

            //menu
            chat = playerControls.interactions.chat.ReadValue<float>() == 1;
            openScoreBoard = playerControls.interactions.openScoreBoard.ReadValue<float>() == 1;

            if(escape != (playerControls.interactions.escape.ReadValue<float>() == 1) && escape == false){
                inGameGUIManager.changeGUIState();
            }
            escape = playerControls.interactions.escape.ReadValue<float>() == 1;
        }
        else{
            Cursor.lockState = CursorLockMode.None;
            mouseDelta = Vector2.zero;
            moveDirection = Vector3.zero;
            jumping = false;

            weaponUse = false;
            throwableUse = false;
            abilityUse = false;
            toolUse = false;

            reloading = false;

            openScoreBoard = playerControls.interactions.openScoreBoard.ReadValue<float>() == 1;
            if(escape != (playerControls.interactions.escape.ReadValue<float>() == 1) && escape == false){
                inGameGUIManager.changeGUIState();
            }
            chat = playerControls.interactions.chat.ReadValue<float>() == 1;
            if(!replayManager.enter && chat){
                replayManager.enter = true;
            }
            escape = playerControls.interactions.escape.ReadValue<float>() == 1;
        }
    }
}
