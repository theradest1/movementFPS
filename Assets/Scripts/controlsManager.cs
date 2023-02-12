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
        mouseDelta = playerControls.camera.mouseDelta.ReadValue<Vector2>();
        
        moveDirection = playerControls.movement.Walk.ReadValue<Vector2>();
        jumping = playerControls.movement.Jump.ReadValue<float>() == 1;
        sprinting = playerControls.movement.Sprint.ReadValue<float>() == 1;

        shooting = playerControls.interactions.shoot.ReadValue<float>() == 1;
        aiming = playerControls.interactions.ADS.ReadValue<float>() == 1;
        reloading = playerControls.interactions.reload.ReadValue<float>() == 1;
    }
}
