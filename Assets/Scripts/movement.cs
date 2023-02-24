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
    public float weaponTravelSpeed;
    GameObject weaponContainer;
    

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .5f, 0f), .5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.5f, 0f), .5f);
    }

    void Start(){
        weaponContainer = GameObject.Find("weapons");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        cam = GameObject.Find("Main Camera");
        look = cam.GetComponent<Look>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        velocityText = GameObject.Find("velocity debug").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate() {
        Vector2 moveDirection = controlsManager.moveDirection;
        if(!controlsManager.jumping){
            ableToJump = true;
        }

        if(Vector2.Distance(lastFootstepPos, new Vector2(transform.position.x, transform.position.z)) >= footstepInterval && isGrounded && !isSliding){
            lastFootstepPos = new Vector2(transform.position.x, transform.position.z);
            serverEvents.sendEvent("ue", "sound", Random.Range(2, 6) + "~" + transform.position + "~1~1");
        }

        if(isGrounded && controlsManager.jumping && ableToJump && !isSliding){
            velocity.y = jumpPower;
            isGrounded = false;
            ableToJump = false;
        }
        Debug.Log(velocity);
        if(isGrounded && controlsManager.crouching){
            if(!isSliding){
                float velocityMag = velocity.magnitude + speedBoostOnSlide;
                velocity = cam.transform.forward * velocityMag;
                Debug.Log("added velocity with magnitude " + velocityMag);
            }
            isSliding = true;
        }
        else{
            isSliding = false;
        }

        if(isGrounded && !isSliding){
            velocity.y = -.0001f;
        }

        if(isGrounded && !isSliding){
            if(controlsManager.sprinting){
                velocity += speed * transform.right * moveDirection.x * sprintMultiplier;
                velocity += speed * transform.forward * moveDirection.y * sprintMultiplier;
            }
            else{
                velocity += speed * transform.right * moveDirection.x;
                velocity += speed * transform.forward * moveDirection.y;
            }
        }
        else if(!isSliding){
            velocity.y += gravity;
            //velocity = Quaternion.AngleAxis(controlsManager.mouseDelta.x * look.LookSpeedHorizontal, Vector3.up) * velocity;
            if(transform.position.y < -100){
                transform.position = new Vector3(0f, 20f, 0f);
            }
        }
        
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

        applyVelocity(velocity);
        
        weaponContainer.transform.position = Vector3.Lerp(weaponContainer.transform.position, cam.transform.position, weaponTravelSpeed);
        
        Vector3 goToPos;
        if(isSliding){
            goToPos = camPosSliding;
        }
        else{
            goToPos = camPosNotSliding;
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, goToPos, camPosSpeed);
    }

    void applyVelocity(Vector3 velocity){
        transform.position += new Vector3(0f, velocity.y, 0f);
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, velocity.y, 0f);
            isGrounded = true;//velocity.y <= 0f;
            velocity.y = 0f;
        }
        else{
            isGrounded = false;
        }

        transform.position += new Vector3(0f, 0f, velocity.z);
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, 0f, velocity.z);
            velocity.z = 0f;
        }
        
        transform.position += new Vector3(velocity.x, 0f, 0f);
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(velocity.x, 0f, 0f);
            velocity.x = 0f;
        }
            
        float yVel = velocity.y;
        velocity.y = 0f;
        velocityText.text = "Velocity: " + (Mathf.Round(velocity.magnitude * 10000f) / 100f);
        velocity.y = yVel;
    }
}
