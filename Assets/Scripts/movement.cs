using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class movement : MonoBehaviour
{
    GameObject cam;
    Look look;
    ControlsManager controlsManager;
    ServerEvents serverEvents;
    TextMeshProUGUI velocityText;

    Vector3 velocity = new Vector3(0f, 0f, 0f);
    bool isGrounded = false;
    bool isSliding = false;
    bool ableToJump = false;

    public LayerMask groundMask;
    public float gravity;

    public float speed;
    public float jumpPower;
    public float stopSpeedGround;
    public float stopSpeedSliding;
    public float stopSpeedAir;
    public float maxSpeed;
    public float sprintMultiplier;
    public float speedBoostOnSlide;

    public float footstepInterval;
    Vector2 lastFootstepPos;

    public Vector3 camPosSliding;
    public float camPosSpeed;
    public Vector3 camPosNotSliding;
    

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .5f, 0f), .5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.5f, 0f), .5f);
    }

    void Start(){
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        cam = GameObject.Find("Main Camera");
        look = cam.GetComponent<Look>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        velocityText = GameObject.Find("velocity debug").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = controlsManager.moveDirection;
        if(!controlsManager.jumping){
            ableToJump = true;
        }
        
        Vector3 goToPos;
        if(isSliding){
            goToPos = camPosSliding;
        }
        else{
            goToPos = camPosNotSliding;
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, goToPos, camPosSpeed * Time.deltaTime);

        if(Vector2.Distance(lastFootstepPos, new Vector2(transform.position.x, transform.position.z)) >= footstepInterval && isGrounded && !isSliding){
            lastFootstepPos = new Vector2(transform.position.x, transform.position.z);
            serverEvents.sendEvent("universalEvent", "sound", Random.Range(2, 6) + "~" + transform.position + "~1~1");
        }

        if(controlsManager.jumping){
            velocity.y = jumpPower;
            isGrounded = false;
            ableToJump = false;
        }
        //Debug.Log(velocity);
        if(isGrounded && controlsManager.crouching){
            if(!isSliding){
                float velocityMag = velocity.magnitude + speedBoostOnSlide;
                velocity = cam.transform.forward * velocityMag;
            }
            isSliding = true;
        }
        else{
            isSliding = false;
        }

        if(isGrounded && !isSliding){
            velocity.y = -1f;
        }

        if(isGrounded && !isSliding){
            if(controlsManager.sprinting){
                velocity += speed * transform.right * moveDirection.x * sprintMultiplier * Time.deltaTime;
                velocity += speed * transform.forward * moveDirection.y * sprintMultiplier * Time.deltaTime;
            }
            else{
                velocity += speed * transform.right * moveDirection.x * Time.deltaTime;
                velocity += speed * transform.forward * moveDirection.y * Time.deltaTime;
            }
        }
        else if(!isSliding){
            velocity.y += gravity * Time.deltaTime;
            //velocity = Quaternion.AngleAxis(controlsManager.mouseDelta.x * look.LookSpeedHorizontal, Vector3.up) * velocity;
            if(transform.position.y < -100){
                transform.position = new Vector3(0f, 20f, 0f);
            }
        }
        
        applyVelocity(velocity);
    }

    void FixedUpdate() {
        if(isGrounded){
            if(isSliding){
                velocity.x *= stopSpeedSliding;
                velocity.z *= stopSpeedSliding;
            }
            else{
                velocity.x *= stopSpeedGround;
                velocity.z *= stopSpeedGround;
            }
        }
        else{
            velocity.x *= stopSpeedAir;
            velocity.z *= stopSpeedAir;
        }
    }

    void applyVelocity(Vector3 velocity){
        transform.position += new Vector3(0f, velocity.y, 0f) * Time.deltaTime;
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, velocity.y, 0f) * Time.deltaTime;
            isGrounded = true;//velocity.y <= 0f;
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
