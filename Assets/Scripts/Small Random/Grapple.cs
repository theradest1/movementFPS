using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    ControlsManager controlsManager;
    ConfigurableJoint joint;
    [HideInInspector]
    public bool grappling = false;
    [HideInInspector]
    public float distanceToGrapple;

    [Header("References:")]
    public GameObject cam;
    public GameObject player;
    public LineRenderer lineRenderer;
    public Rigidbody rb;

    [Header("joint settings:")]
    public LayerMask stopFrom;
    public float maxDist;
    Vector3 connectPos;
    public float climbSpeed;

    private void Start() {
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        //joint = null;
    }

    public void attach(){
        if(Physics.Raycast(transform.position, cam.transform.forward, out RaycastHit hitInfo, maxDist, stopFrom)){
            connectPos = hitInfo.point;
            distanceToGrapple = Vector3.Distance(cam.transform.position, connectPos);

            joint = player.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = Vector3.zero;
            joint.connectedAnchor = connectPos;
            //joint.linearLimitSpring.spring = springForce;
            //joint.damper = damper;

            joint.xMotion = ConfigurableJointMotion.Limited;
            joint.yMotion = ConfigurableJointMotion.Limited;
            joint.zMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = distanceToGrapple;
            joint.linearLimit = softJointLimit;
            //joint.massScale = massScale;


            //joint.maxDistance = Vector3.Distance(cam.transform.position, connectPos);
            //joint.minDistance = Vector3.Distance(cam.transform.position, connectPos);*/

            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, connectPos);

            grappling = true;
        }
    }

    private void Update() {


        if(!grappling && controlsManager.grappling){
            attach();
        }
        else if(grappling && !controlsManager.grappling){
            Destroy(joint);
            grappling = false;
        }
        if(grappling){
            SoftJointLimit softJointLimit = new SoftJointLimit();
            distanceToGrapple -= climbSpeed * Time.deltaTime;
            softJointLimit.limit = distanceToGrapple;
            joint.linearLimit = softJointLimit;

            if(Physics.Raycast(cam.transform.position, connectPos - cam.transform.position, out RaycastHit hitInfo, Vector3.Distance(cam.transform.position, connectPos), stopFrom)){
                distanceToGrapple -= Vector3.Distance(connectPos, hitInfo.point);
                softJointLimit.limit = distanceToGrapple;
                joint.linearLimit = softJointLimit;
                connectPos = hitInfo.point;
                joint.connectedAnchor = connectPos;
                lineRenderer.SetPosition(0, connectPos);
            }
        }
    }

    void LateUpdate() {
        //Debug.Log(Vector3.Distance(player.transform.position, connectPos));
        if(grappling){
            /*if(Vector3.Distance(player.transform.position, connectPos) > distanceToGrapple - .1f){
                joint.spring = 100f;
            }
            else if(Vector3.Distance(player.transform.position, connectPos) < distanceToGrapple + .05f){
                joint.spring = 0f;
            }*/
            lineRenderer.SetPosition(1, player.transform.position);
        }
        else if(lineRenderer.enabled){
            lineRenderer.enabled = false;
        }
    }
}
