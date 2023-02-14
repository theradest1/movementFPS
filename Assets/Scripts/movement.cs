using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class movement : MonoBehaviour
{
    GameObject cam;
    look lookScript;
    controlsManager controlsManagerScript;
    ServerEvents serverEvents;
    TextMeshProUGUI velocityText;

    Vector3 velocity = new Vector3(0f, 0f, 0f);
    bool isGrounded = true;

    public LayerMask groundMask;
    public float gravity;

    public float speed;
    public float jumpPower;
    public float stopSpeedGround;
    public float stopSpeedAir;
    public float maxSpeed;
    public float sprintMultiplier;

    public float footstepInterval;
    public Vector2 lastFootstepPos;

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .5f, 0f), .5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.5f, 0f), .5f);
    }

    void Start(){
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        cam = GameObject.Find("Main Camera");
        lookScript = cam.GetComponent<look>();
        controlsManagerScript = GameObject.Find("manager").GetComponent<controlsManager>();
        velocityText = GameObject.Find("velocity debug").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = controlsManagerScript.moveDirection;

        if(Vector2.Distance(lastFootstepPos, new Vector2(transform.position.x, transform.position.z)) >= footstepInterval && isGrounded){
            lastFootstepPos = new Vector2(transform.position.x, transform.position.z);
            serverEvents.sendEvent("universalEvent", "sound", Random.Range(2, 6) + "~" + transform.position + "~1~1");
        }

        if(isGrounded && controlsManagerScript.jumping){
            velocity.y = jumpPower;
        }

        if(isGrounded && !controlsManagerScript.sprinting){
            velocity += speed * transform.right * moveDirection.x * Time.deltaTime;
            velocity += speed * transform.forward * moveDirection.y * Time.deltaTime;
        }
        else if(isGrounded && controlsManagerScript.sprinting){
            velocity += speed * transform.right * moveDirection.x * sprintMultiplier * Time.deltaTime;
            velocity += speed * transform.forward * moveDirection.y * sprintMultiplier * Time.deltaTime;
        }
        else{
            velocity.y += gravity * Time.deltaTime;
            velocity = Quaternion.AngleAxis(controlsManagerScript.mouseDelta.x * lookScript.lookSpeedHorizontal, Vector3.up) * velocity;
            if(transform.position.y < -100){
                transform.position = new Vector3(0f, 20f, 0f);
            }
        }
        
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
        
        transform.position += new Vector3(0f, velocity.y, 0f) * Time.deltaTime;
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, velocity.y, 0f) * Time.deltaTime;
            isGrounded = velocity.y <= 0f;
            velocity.y = 0f;
        }
        else{
            isGrounded = false;
        }

        transform.position += new Vector3(0f, 0f, velocity.z) * Time.deltaTime;
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, 0f, velocity.z) * Time.deltaTime;
            velocity.z = 0f;
        }
        
        transform.position += new Vector3(velocity.x, 0f, 0f) * Time.deltaTime;
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(velocity.x, 0f, 0f) * Time.deltaTime;
            velocity.x = 0f;
        }
        
        velocityText.text = "Velocity: " + (Mathf.Round(velocity.magnitude * 10000f) / 100f);
    }
}
