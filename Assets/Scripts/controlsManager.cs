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
        moveDirection = playerControls.movement.Walk.ReadValue<Vector2>();
        mouseDelta = playerControls.camera.mouseDelta.ReadValue<Vector2>();
        jumping = playerControls.movement.Jump.ReadValue<float>() == 1;
        shooting = playerControls.interactions.shoot.ReadValue<float>() == 1;
        aiming = playerControls.interactions.ADS.ReadValue<float>() == 1;
    }
}
