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
    ServerComm serverComm;
    PlayerManager playerManager;

    //Vector3 velocity = new Vector3(0f, 0f, 0f);

    public bool isGrounded = false;
    public bool isSliding = false;
    public bool ableToJump = false;

    public LayerMask groundMask;

    public float speed;
    public float speedMultiplierFromWeapon = 1f;
    public float jumpPower;
    public float stopSpeedGround;
    public float stopSpeedSliding;
    public float stopSpeedAir;
    public float maxSpeed;
    public float sprintMultiplier;
    public float speedBoostOnDash;

    public float footstepInterval;
    Vector2 lastFootstepPos;

    public Vector3 camPosSliding;
    public float camPosSpeed;
    public Vector3 camPosNotSliding;
    public float weaponTravelSpeed;
    GameObject weaponContainer;

    public float ableToDash = 0f;
    public float dashTimout;

    public ClassInfo currentClass;

    public float minHeight;
    int launchAttempts = 0;
    public int maxLaunchAttempts = 3;
    public float bounceBackCorrection = 1.05f;
    public Rigidbody weaponContainerRB;

    public void launchTo(Vector3 goToPos){
        launchAttempts++;
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
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
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
        Vector3 goToPos;
        if(isSliding){
            goToPos = camPosSliding;
        }
        else{
            goToPos = camPosNotSliding;
        }
        cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, goToPos, camPosSpeed * Time.deltaTime);
    }

    void FixedUpdate() {
        weaponContainerRB.AddForce((cam.transform.position - weaponContainer.gameObject.transform.position)*weaponTravelSpeed); 

        //rb.velocity += Vector3.up * gravity;
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
            if(transform.position.y < minHeight){
                if(launchAttempts == maxLaunchAttempts){
                    transform.position = new Vector3(0f, 100f, 0f);
                    serverEvents.sendEvent("ue", "death", serverComm.ID + "");
                }
                else{
                    rb.velocity *= -bounceBackCorrection;
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
            rb.velocity = new Vector3(rb.velocity.x * stopSpeedAir, rb.velocity.y, rb.velocity.z * stopSpeedAir);
        }
        
        if(Physics.CheckSphere(transform.position + new Vector3(0f, -.6f, 0f), .45f, groundMask)){
            isGrounded = true;
            launchAttempts = 0;
        }
        else{
            isGrounded = false;
        }

        if(controlsManager.dashing && !isSliding){
            if(ableToDash <= 0 && isGrounded){
                float velocityMag = rb.velocity.magnitude + speedBoostOnDash;
                rb.velocity = cam.transform.forward * velocityMag;
                ableToDash = dashTimout;
            }
        }

        velocityText.text = "Velocity: " + (Mathf.Round(new Vector3(rb.velocity.x, 0f, rb.velocity.z).magnitude * 100f) / 100f);
    }
}
