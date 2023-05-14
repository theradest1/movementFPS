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
    public bool ableToJump = false;

    [Header("Controlled by server:")]
    public float speed;
    public float inAirMultiplier;
    //public float grapplingMultiplier;
    public float jumpPower;
    public float stopSpeedGround;
    public float footstepInterval;
    public float minHeight;
    public int maxLaunchAttempts = 3;

    [Header("Basic:")]
    public LayerMask groundMask;

    [Header("Footsteps:")]
    Vector2 lastFootstepPos;

    [Header("Cam:")]
    public float camPosSpeed;
    public Vector3 camPosNotSliding;

    [Header("Weapon:")]
    //public float weaponTravelSpeed;
    public float weaponDistanceMult;
    public float weaponDistanceMultADS;
    //public float weaponDistanceMultVertical;
    public float speedMultiplierFromWeapon = 1f;
    public GameObject weaponContainer;
    public float weaponContainerSpeed;

    [Header("Rebound:")]
    int launchAttempts = 0;

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
        //grapple = GameObject.Find("Player").GetComponent<Grapple>();
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
        Vector3 goToPos = camPosNotSliding;
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, goToPos, camPosSpeed * Time.deltaTime);
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

        if(Vector2.Distance(lastFootstepPos, new Vector2(transform.position.x, transform.position.z)) >= footstepInterval && isGrounded){
            lastFootstepPos = new Vector2(transform.position.x, transform.position.z);
            serverEvents.sendEvent("ue", "sound", Random.Range(2, 6) + "~" + transform.position + "~1~1");
        }

        if(controlsManager.jumping){
            if(isGrounded && ableToJump){
                rb.velocity = new Vector3(rb.velocity.x, jumpPower, rb.velocity.z);
                isGrounded = false;
            }
            ableToJump = false;
        }

        if(isGrounded){
            //movement
            rb.AddForce(speed * transform.right * moveDirection.x * speedMultiplierFromWeapon);
            rb.AddForce(speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon);
            //friction
            rb.velocity = new Vector3(rb.velocity.x * stopSpeedGround, rb.velocity.y, rb.velocity.z * stopSpeedGround);
        }
        else{
            rb.AddForce(speed * transform.right * moveDirection.x * speedMultiplierFromWeapon * inAirMultiplier);
            rb.AddForce(speed * transform.forward * moveDirection.y * speedMultiplierFromWeapon * inAirMultiplier);

            //launching
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
        velocityText.text = "Velocity: " + (Mathf.Round(new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude * 100f) / 100f);
    }
}