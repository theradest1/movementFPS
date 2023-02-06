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

    public controlsManager controlsManagerScript;

    public float gravity;
    public Vector3 velocity = new Vector3(0f, 0f, 0f);

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .5f, 0f), .5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.5f, 0f), .5f);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);
        //bool colliding = Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask);
        //Debug.Log("Colliding: " + colliding);
        Vector2 moveDirection = controlsManagerScript.moveDirection;

        if(isGrounded && controlsManagerScript.jumping){
            velocity.y = jumpPower;
            //isGrounded = false;
        }

        if(isGrounded){
            velocity.x += speed * moveDirection.x * Time.deltaTime;
            velocity.z += speed * moveDirection.y * Time.deltaTime;
            //velocity.y = Mathf.Clamp(velocity.y, 0f, 99999999999f);
            //velocity.x *= stopSpeedGround;
            //velocity.z *= stopSpeedGround;
        }
        else{
            velocity.y += gravity * Time.deltaTime;
            
        }

        //Debug.Log(velocity);
        applyVelocity(velocity);
    }

    void FixedUpdate() {
        if(!isGrounded){
            velocity.x *= stopSpeedAir;
            velocity.z *= stopSpeedAir;
        }
        else{
            velocity.x *= stopSpeedGround;
            velocity.z *= stopSpeedGround;
        }
    }

    void applyVelocity(Vector3 velocity){
        //transform.position += transform.forward * velocity.z + transform.right * velocity.x + new Vector3(0f, velocity.y, 0f);
        transform.position += transform.forward * velocity.z;
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= transform.forward * velocity.z;
        }
        transform.position += transform.right * velocity.x;
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= transform.right * velocity.x;
        }
        transform.position += new Vector3(0f, velocity.y, 0f);
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, velocity.y, 0f);
        }
    }
}
