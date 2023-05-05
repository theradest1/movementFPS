using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class movement : MonoBehaviour
{
    //references
    GameObject cam;
    Look look;
    //GameObject weaponContainer;
    Rigidbody rb;
    ControlsManager controlsManager;
    ServerEvents serverEvents;
    ServerComm serverComm;
    PlayerManager playerManager;
    Grapple grapple;

    //Vector3 velocity = new Vector3(0f, 0f, 0f);

    [Header("Debug:")]
    public TextMeshProUGUI velocityText;
    public TextMeshProUGUI FPStext;
    public bool isGrounded = false;
    public bool isSliding = false;
    public bool ableToJump = false;

    [Header("Basic:")]
    public LayerMask groundMask;
    public float speed;
    public float inAirMultiplier;
    public float grapplingMultiplier;
    public float jumpPower;
    public float stopSpeedGround;
    public float stopSpeedSliding;
    public float sprintMultiplier;
    
    [Header("Dash (old):")]
    public float ableToDash = 0f;
    public float dashTimout;
    public float speedBoostOnDash;
    public float velocityMagVertical;

    [Header("Footsteps:")]
    public float footstepInterval;
    Vector2 lastFootstepPos;

    [Header("Cam:")]
    public Vector3 camPosSliding;
    public float camPosSpeed;
    public Vector3 camPosNotSliding;

    [Header("Weapon:")]
    //public float weaponTravelSpeed;
    public float weaponDistanceMult;
    public float weaponDistanceMultADS;
    //public float weaponDistanceMultVertical;
    public float speedMultiplierFromWeapon = 1f;
    public GameObject weaponContainer;




    [Header("Rebound:")]
    public float minHeight;
    int launchAttempts = 0;
    public int maxLaunchAttempts = 3;
    public float ableToDashLengthInAirMult = .1f;

    [Header("Dash (new):")]
    public float dashTime;
    public float dashMultiplier;
    public float dashCooldown;
    float dashCooldownTimer;
    public bool dashing;

    public void launchTo(Vector3 goToPos){
        Vector3 toTarget = goToPos - transform.position;

        //quadratic
        float gSquared = Physics.gravity.sqrMagnitude;
        float T = Mathf.Sqrt(Mathf.Sqrt(toTarget.sqrMagnitude * 4f/gSquared));

        //convert from time-to-hit to a launch velocity:
        Vector3 newVelocity = toTarget / T - Physics.gravity * T / 2f;
        rb.AddForce(newVelocity, ForceMode.VelocityChange);
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.6f, 0f), .45f);
    }

    void Start(){
        grapple = GameObject.Find("Player").GetComponent<Grapple>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        //weaponContainer = GameObject.Find("weapons");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        cam = GameObject.Find("Main Camera");
        look = cam.GetComponent<Look>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FPStext.text = "FPS: " + (Mathf.Round(1/Time.deltaTime));
        Vector3 goToPos;
        if(isSliding){
            goToPos = camPosSliding;
        }
        else{
            goToPos = camPosNotSliding;
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, goToPos, camPosSpeed * Time.deltaTime);
        if(controlsManager.aiming){
            weaponContainer.transform.position -= rb.velocity * weaponDistanceMultADS * Time.deltaTime;
        }
        else{
            weaponContainer.transform.position -= rb.velocity * weaponDistanceMult * Time.deltaTime;
        }

        dashCooldownTimer -= Time.deltaTime;
        if(controlsManager.dashing && dashCooldownTimer <= 0){
            StartCoroutine(dash());
            dashCooldownTimer = dashCooldown;
        }
    }

    IEnumerator dash(){
        dashing = true;
        yield return new WaitForSeconds(dashTime);
        dashing = false;
        yield return null;
    }

    void FixedUpdate() {
        if(Physics.CheckSphere(transform.position + new Vector3(0f, -.6f, 0f), .45f, groundMask)){
            isGrounded = true;
            launchAttempts = 0;
        }
        else{
            isGrounded = false;
        }
        //rb.velocity += Vector3.up * gravity;
        //weaponContainer.transform.localPosition = Vector3.Lerp(weaponContainer.transform.localPosition, Vector3.zero, weaponTravelSpeed);
        Vector2 moveDirection = controlsManager.moveDirection;
        if(!controlsManager.jumping){
            ableToJump = true;
        }

        if(Vector2.Distance(lastFootstepPos, new Vector2(transform.position.x, transform.position.z)) >= footstepInterval && isGrounded && !isSliding){
            lastFootstepPos = new Vector2(transform.position.x, transform.position.z);
            serverEvents.sendEvent("ue", "sound", Random.Range(2, 6) + "~" + transform.position + "~1~1");
        }

        if(controlsManager.jumping){
            if(isGrounded && ableToJump && !isSliding){
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
                isGrounded = false;
            }
            ableToJump = false;
        }

        if(isGrounded && controlsManager.crouching){
            isSliding = true;
        }
        else{
            isSliding = false;
        }

        //if(isGrounded && !isSliding){
        //    rb.velocity = Vector3.up * -.0001f;
        //}
        //weaponContainer.transform.position -= new Vector3(0f, rb.velocity.y * weaponDistanceMultVertical, 0f);
        if(isGrounded && !isSliding){
            if(controlsManager.sprinting){
                rb.AddForce(speed * transform.right * moveDirection.x * sprintMultiplier * speedMultiplierFromWeapon);
                rb.AddForce(speed * transform.forward * moveDirection.y * sprintMultiplier * speedMultiplierFromWeapon);
                //weaponContainer.transform.position -= speed * transform.right * moveDirection.x * sprintMultiplier * speedMultiplierFromWeapon * currentClass.speedMult * weaponDistanceMult;
                //weaponContainer.transform.position -= speed * transform.forward * moveDirection.y * sprintMultiplier * speedMultiplierFromWeapon * currentClass.speedMult * weaponDistanceMult;
            }
            else{
                rb.AddForce(speed * transform.right * moveDirection.x * speedMultiplierFromWeapon);
                rb.AddForce(speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon);
                //weaponContainer.transform.position -= speed * transform.right * moveDirection.x * speedMultiplierFromWeapon * currentClass.speedMult * weaponDistanceMult;
                //weaponContainer.transform.position -= speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon * currentClass.speedMult * weaponDistanceMult;
            }
        }
        else if(!isSliding){
            if(grapple.grappling){
                rb.AddForce(speed * transform.right * moveDirection.x * speedMultiplierFromWeapon * grapplingMultiplier);
                rb.AddForce(speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon * grapplingMultiplier);
            }
            else{
                rb.AddForce(speed * transform.right * moveDirection.x * speedMultiplierFromWeapon * inAirMultiplier);
                rb.AddForce(speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon * inAirMultiplier);
            }

            if(transform.position.y < minHeight){
                if(launchAttempts == maxLaunchAttempts){
                    rb.position = new Vector3(0f, 100f, 0f);
                    serverEvents.sendEvent("ue", "death", serverComm.ID + "");
                }
                else{
                    rb.velocity *= -1;
                    launchAttempts++;
                }
            }
        }
        
        if(isGrounded){
            ableToDash -= Time.deltaTime;
            if(isSliding){
                rb.velocity = new Vector3(rb.velocity.x * stopSpeedSliding, rb.velocity.y, rb.velocity.z * stopSpeedSliding);
            }
            else{
                rb.velocity = new Vector3(rb.velocity.x * stopSpeedGround, rb.velocity.y, rb.velocity.z * stopSpeedGround);
            }
        }
        else{
            ableToDash -= Time.deltaTime * ableToDashLengthInAirMult;
        }
        

        if(controlsManager.dashing && !isSliding){
            if(ableToDash <= 0){// && isGrounded){
                //grapple.attach();
                //float velocityMag = rb.velocity.magnitude + speedBoostOnDash;
                //rb.velocity = new Vector3(0f, cam.transform.forward.y, 0f) * velocityMagVertical * velocityMag + moveDirection.x * transform.right * velocityMag + moveDirection.y * transform.forward * velocityMag;
                //ableToDash = dashTimout;
            }
        }
        velocityText.text = "Velocity: " + (Mathf.Round(new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude * 100f) / 100f);

        //dashing
        if(dashing){
            rb.MovePosition(transform.position + moveDirection.x * dashMultiplier * transform.right * speed + moveDirection.y * dashMultiplier * transform.forward * speed);
        }
    }
}