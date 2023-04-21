using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    Vector3 connection;
    //public float maxLength;
    //public float jointDensity;
    List<ConfigurableJoint> joints = new List<ConfigurableJoint>();
    public GameObject startJoint;
    public GameObject player;
    public float distanceMult;
    public GameObject jointPrefab;
    public GameObject ropeParent;

    private void Start() {
        player = GameObject.Find("Player");
    }

    private void Update() {
        if(joints.Count > 0){
            joints[joints.Count - 1].transform.position = connection;
            startJoint.transform.position = player.transform.position;
        }
    }

    public void changeLength(float reduction, Vector3 newConnectPos){
        joints[joints.Count - 1].transform.position = newConnectPos;
        SoftJointLimit softJointLimit = new SoftJointLimit();
        softJointLimit.limit = (joints[0].linearLimit.limit - reduction/(joints.Count + 1))/(joints.Count + 1) * distanceMult;
        connection = newConnectPos;
        foreach(ConfigurableJoint joint in joints){
            joint.linearLimit = softJointLimit;
        }
    }

    public void makeRope(int segments, Vector3 _connection){
        connection = _connection;

        destroyRope();

        startJoint.SetActive(true);
        float distancePerSegment = (Vector3.Distance(player.transform.position, connection))/(segments + 2)* distanceMult;
        Vector3 stepPos = Vector3.Normalize(connection - player.transform.position) * distancePerSegment;
        Vector3 spawnPos = player.transform.position + stepPos;
        Rigidbody linkedRB = startJoint.GetComponent<Rigidbody>();
        
        SoftJointLimit softJointLimit = new SoftJointLimit();
        softJointLimit.limit = distancePerSegment;
        
        for(int i = 0; i <= segments; i++){
            ConfigurableJoint newJoint = Instantiate(jointPrefab, spawnPos, Quaternion.identity, ropeParent.transform).GetComponent<ConfigurableJoint>();
            newJoint.linearLimit = softJointLimit;
            newJoint.connectedBody = linkedRB;
            newJoint.gameObject.GetComponent<DrawRope>().objectToFollow = linkedRB.gameObject.transform;
            linkedRB = newJoint.gameObject.GetComponent<Rigidbody>();
            joints.Add(newJoint);
            spawnPos += stepPos;
        }

        linkedRB = joints[joints.Count - 1].gameObject.GetComponent<Rigidbody>();
        linkedRB.isKinematic = true;
        linkedRB.useGravity = false;
    }

    public void destroyRope(){
        startJoint.SetActive(false);
        if(joints.Count > 0){
            foreach(ConfigurableJoint joint in joints){
                Destroy(joint.gameObject);
            }
        }
        joints = new List<ConfigurableJoint>();
    }
}
