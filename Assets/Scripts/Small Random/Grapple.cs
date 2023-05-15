using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    ControlsManager controlsManager;
    Rope grappleRope;
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
    public float minDistToRedoRope;
    public int ropeDetail;

    private void Start() {
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        grappleRope = GameObject.Find("grapple rope parent").GetComponent<Rope>();
        //joint = null;
    }

    public void attach(){
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, maxDist, stopFrom)){
            grappling = true;
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
            //grappleRope.gameObject.SetActive(true);
            grappleRope.makeRope(ropeDetail, connectPos);
            //joint.maxDistance = Vector3.Distance(cam.transform.position, connectPos);
            //joint.minDistance = Vector3.Distance(cam.transform.position, connectPos);*/

            //lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, connectPos);

        }
    }

    private void Update() {
        if(!grappling && controlsManager.toolUse){
            attach();
        }
        else if(grappling && !controlsManager.toolUse){
            Destroy(joint);
            grappleRope.destroyRope();
            grappling = false;
        }
        if(grappling){
            /*
            distanceToGrapple -= climbSpeed * Time.deltaTime;
            softJointLimit.limit = distanceToGrapple;
            joint.linearLimit = softJointLimit;*/


            if(Physics.Raycast(cam.transform.position, connectPos - cam.transform.position, out RaycastHit hitInfo, Vector3.Distance(cam.transform.position, connectPos), stopFrom)){
                SoftJointLimit softJointLimit = new SoftJointLimit();
                distanceToGrapple -= Vector3.Distance(connectPos, hitInfo.point);
                softJointLimit.limit = distanceToGrapple;
                joint.linearLimit = softJointLimit;
                Vector3 pastConnect = connectPos;
                connectPos = hitInfo.point;
                joint.connectedAnchor = connectPos;
                lineRenderer.SetPosition(0, connectPos);
                if(Vector3.Distance(pastConnect, connectPos) > minDistToRedoRope){
                    //grappleRope.makeRope(ropeDetail, connectPos);
                    grappleRope.changeLength(Vector3.Distance(pastConnect, connectPos), connectPos);
                    //grappleRope.connection = connectPos;
                }
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

    public Vector3 getLookPos(){
        if(!grappling && Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hitInfo, maxDist, stopFrom)){
            return hitInfo.point;
        }
        else{
            return new Vector3(99999999, 99999999, 999999999);
        }
    }
}
