using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class movement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float stopSpeedGround;
    public float stopSpeedAir;

    public Transform groundCheck;
    public LayerMask groundMask;
    public bool isGrounded;
    public float groundDistance;

    public controlsManager controlsManagerScript;

    public float gravity;
    public Vector3 velocity = new Vector3(0f, 0f, 0f);

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        Vector2 moveDirection = controlsManagerScript.moveDirection;

        if(isGrounded && controlsManagerScript.jumping){
            isGrounded = false;
            velocity.y += jumpPower;
        }

        if(isGrounded){
            velocity.x += speed * moveDirection.x * Time.deltaTime;
            velocity.z += speed * moveDirection.y * Time.deltaTime;
            velocity.y = Mathf.Clamp(velocity.y, 0f, 99999999999f);
            velocity.x *= stopSpeedGround;
            velocity.z *= stopSpeedGround;
        }
        else{
            velocity.y += gravity * Time.deltaTime;
            velocity.x *= stopSpeedAir;
            velocity.z *= stopSpeedAir;
        }

        //Debug.Log(velocity);
        transform.position += transform.forward * velocity.z + transform.right * velocity.x + new Vector3(0f, velocity.y, 0f);

    }
}
