using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class movement : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public float stopSpeedGround;
    public float stopSpeedAir;

    public Transform groundCheck;
    public LayerMask groundMask;
    public bool isGrounded = true;

    public controlsManager controlsManagerScript;

    public float gravity;
    public Vector3 velocity = new Vector3(0f, 0f, 0f);

    public TextMeshProUGUI velocityText;
    public bool attatched;
    public bool ableToAttatch = true;
    public bool ableToJumpOff = true;
    public float ableToJumpOffTimer = 0f;
    public float timeBeforeAbleToJumpOff = .2f;
    public float ableToAttatchTimer = 0f;
    public float timeBeforeAbleToAttatch = 2f;
    public float wallJumpVerticalPower;
    public float wallJumpPower;

    public GameObject cam;
    public look lookScript;
    public float lookSpeedHorizontal;
    public float speedReductionPerDegree;
    public float minDegreesToEffectSpeed;
    public float inAirStrafingSpeed;
    public float maxSpeed;


    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, .5f, 0f), .5f);
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.5f, 0f), .5f);
    }

    void Start(){
        lookSpeedHorizontal = lookScript.lookSpeedHorizontal;
    }

    // Update is called once per frame
    void Update()
    {
        //isGrounded = Physics.CheckSphere(groundCheck.position, .5f, groundMask);
        //bool colliding = Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask);
        //Debug.Log("Colliding: " + colliding);

        Vector2 moveDirection = controlsManagerScript.moveDirection;

        if(isGrounded && controlsManagerScript.jumping){
            velocity.y = jumpPower;
            //isGrounded = false;
        }

        if(attatched && controlsManagerScript.jumping && ableToJumpOff){
            attatched = false;
            velocity.y = wallJumpVerticalPower;
            velocity += cam.transform.forward * wallJumpPower;
            ableToAttatchTimer = timeBeforeAbleToAttatch;
        }

        ableToAttatchTimer -= Time.deltaTime;
        if(ableToAttatchTimer <= 0f && !attatched){
            ableToAttatch = true;
        }
        ableToJumpOffTimer -= Time.deltaTime;
        if(ableToJumpOffTimer <= 0f && attatched){
            ableToJumpOff = true;
        }

        if(isGrounded){
            velocity += speed * transform.right * moveDirection.x * Time.deltaTime;
            velocity += speed * transform.forward * moveDirection.y * Time.deltaTime;
        }
        else if(attatched == false){
            velocity.y += gravity * Time.deltaTime;
            velocity = Quaternion.AngleAxis(controlsManagerScript.mouseDelta.x * lookSpeedHorizontal, Vector3.up) * velocity;
            if(controlsManagerScript.mouseDelta.x * lookSpeedHorizontal * Time.deltaTime > minDegreesToEffectSpeed){
                velocity *= Mathf.Pow(speedReductionPerDegree, controlsManagerScript.mouseDelta.x * lookSpeedHorizontal);
                Debug.Log("reduced speed");
            }
            
        }
        else{
            velocity.y = 0f;
        }

        //Debug.Log(velocity);
        float yVel = velocity.y;
        velocity.y = 0f;
        Debug.Log("-------------------");
        Debug.Log(velocity.magnitude);
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        Debug.Log(velocity.magnitude);
        velocity.y = yVel;
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
        transform.position += new Vector3(0f, velocity.y, 0f);
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, velocity.y, 0f);
            isGrounded = velocity.y <= 0f;
            ableToAttatch = isGrounded;
            velocity.y = 0f;
        }
        else{
            isGrounded = false;
        }
        velocityText.text = (Mathf.Round(velocity.magnitude * 10000f) / 100f) + "";

        transform.position += new Vector3(0f, 0f, velocity.z);
        bool possibleAttatched = false; //just so you don't randomly get flung off because update is called in between apply velocities (not sure how)
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(0f, 0f, velocity.z);
            velocity.z = 0f;
            possibleAttatched = isGrounded == false;
        }
        
        transform.position += new Vector3(velocity.x, 0f, 0f);
        if(Physics.CheckCapsule(transform.position + new Vector3(0f, .5f, 0f), transform.position + new Vector3(0f, -.5f, 0f), .5f, groundMask)){
            transform.position -= new Vector3(velocity.x, 0f, 0f);
            velocity.x = 0f;
            possibleAttatched = isGrounded == false;
        }
        if(possibleAttatched && ableToAttatch && !attatched){
            attatched = true;
            ableToJumpOff = false;
            ableToJumpOffTimer = timeBeforeAbleToJumpOff;
            ableToAttatch = false;
        }
        if(!possibleAttatched){
            attatched = possibleAttatched;
        }
    }
}
