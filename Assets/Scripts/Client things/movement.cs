using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class movement : MonoBehaviour
{
    GameObject cam;
    Look look;
    Rigidbody rb;
    ControlsManager controlsManager;
    ServerEvents serverEvents;
    TextMeshProUGUI velocityText;
    TextMeshProUGUI FPStext;

    //Vector3 velocity = new Vector3(0f, 0f, 0f);

    public bool isGrounded = false;
    public bool isSliding = false;
    public bool ableToJump = false;

    public LayerMask groundMask;
    public float gravity;

    public float speed;
    public float speedMultiplierFromWeapon = 1f;
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

    public ClassInfo currentClass;

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .5f, 0f), .5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.5f, 0f), .5f);
    }

    void Start(){
        rb = this.gameObject.GetComponent<Rigidbody>();
        FPStext = GameObject.Find("FPS debug").GetComponent<TextMeshProUGUI>();
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
        FPStext.text = "FPS: " + (Mathf.Round(1/Time.deltaTime));
        weaponContainer.transform.position = Vector3.Lerp(weaponContainer.transform.position, cam.transform.position, weaponTravelSpeed * Time.deltaTime);
    }

    void FixedUpdate() {
        rb.velocity += Vector3.up * gravity;
        Vector2 moveDirection = controlsManager.moveDirection;
        if(!controlsManager.jumping){
            ableToJump = true;
        }

        if(Vector2.Distance(lastFootstepPos, new Vector2(transform.position.x, transform.position.z)) >= footstepInterval && isGrounded && !isSliding){
            lastFootstepPos = new Vector2(transform.position.x, transform.position.z);
            serverEvents.sendEvent("ue", "sound", Random.Range(2, 6) + "~" + transform.position + "~1~1");
        }

        if(isGrounded && controlsManager.jumping && ableToJump && !isSliding){
            rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
            isGrounded = false;
            ableToJump = false;
        }

        if(isGrounded && controlsManager.crouching){
            if(!isSliding){
                float velocityMag = rb.velocity.magnitude + speedBoostOnSlide;
                rb.velocity = cam.transform.forward * velocityMag;
            }
            isSliding = true;
        }
        else{
            isSliding = false;
        }

        //if(isGrounded && !isSliding){
        //    rb.velocity = Vector3.up * -.0001f;
        //}

        if(isGrounded && !isSliding){
            if(controlsManager.sprinting){
                rb.velocity += speed * transform.right * moveDirection.x * sprintMultiplier * speedMultiplierFromWeapon * currentClass.speedMult;
                rb.velocity += speed * transform.forward * moveDirection.y * sprintMultiplier * speedMultiplierFromWeapon * currentClass.speedMult;
            }
            else{
                rb.velocity += speed * transform.right * moveDirection.x * speedMultiplierFromWeapon * currentClass.speedMult;
                rb.velocity += speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon * currentClass.speedMult;
            }
        }
        else if(!isSliding){
            //rb.velocity.y += gravity;
            //velocity = Quaternion.AngleAxis(controlsManager.mouseDelta.x * look.LookSpeedHorizontal, Vector3.up) * velocity;
            if(transform.position.y < -100){
                transform.position = new Vector3(0f, 20f, 0f);
            }
        }
        
        if(isGrounded){
            if(isSliding){
                rb.velocity = new Vector3(rb.velocity.x * stopSpeedSliding, rb.velocity.y, rb.velocity.z * stopSpeedSliding);
            }
            else{
                rb.velocity = new Vector3(rb.velocity.x * stopSpeedGround, rb.velocity.y, rb.velocity.z * stopSpeedGround);
            }
        }
        else{
            rb.velocity = new Vector3(rb.velocity.x * stopSpeedAir, rb.velocity.y, rb.velocity.z * stopSpeedAir);
        }

        //applyVelocity(velocity);
        
        Vector3 goToPos;
        if(isSliding){
            goToPos = camPosSliding;
        }
        else{
            goToPos = camPosNotSliding;
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, goToPos, camPosSpeed);

        if(Physics.CheckSphere(transform.position + new Vector3(0f, -.51f, 0f), .5f, groundMask)){
            isGrounded = true;
        }
        else{
            isGrounded = false;
        }

        velocityText.text = "Velocity: " + (Mathf.Round(new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude * 100f) / 100f);
    }

    /*void applyVelocity(Vector3 velocity){
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
    }*/
}
