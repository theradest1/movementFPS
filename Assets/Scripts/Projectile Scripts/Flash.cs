using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public Rigidbody rb;
    public float timeToExplode;
    public int bangSound;
    public int bounceSound;
    public LayerMask stopFrom;
    ProjectileFunctions projectileFunctions;
    public Collider coll;

    public void setInfo(Vector3 givenVelocity, ProjectileFunctions givenProjectileFunctions){
        rb.velocity = givenVelocity;
        projectileFunctions = givenProjectileFunctions;
        Invoke("explode", timeToExplode);
    }

    private void Start() {
        Physics.IgnoreCollision(coll, projectileFunctions.playerColl, true);
    }

    void explode(){
        projectileFunctions.soundManager.playSound(bangSound, transform.position, 1f, 1f);
        
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(projectileFunctions.playerCamComponent);
        if(GeometryUtility.TestPlanesAABB(planes , coll.bounds) && !Physics.Raycast(transform.position, projectileFunctions.playerCam.transform.position - transform.position, Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position), stopFrom)){
            projectileFunctions.flashImage.color = new Color(1, 1, 1, 1);
        }
        //Debug.Log("bang (flash)");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll){
        projectileFunctions.soundManager.playSound(bounceSound, transform.position, 1f, 2f);
    }
}
