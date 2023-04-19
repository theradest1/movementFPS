using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public Vector3 connection;
    //public float maxLength;
    //public float jointDensity;
    List<ConfigurableJoint> joints;
    //public GameObject startJoint;
    public GameObject player;
    public float distanceReduction;

    private void Start() {
        player = GameObject.Find("Player");
    }

    private void Update() {
        joints[joints.Count - 1].transform.position = connection;
        //startJoint.transform.position = player.transform.position;
    }

    public void changeLength(float _maxLength){
        SoftJointLimit softJointLimit = new SoftJointLimit();
        softJointLimit.limit = _maxLength/(joints.Count + 1) - distanceReduction;
        foreach(ConfigurableJoint joint in joints){
            joint.linearLimit = softJointLimit;
        }
        Vector3 stepPos = (connection - player.transform.position)/(joints.Count + 1);
        Vector3 spawnPos;
        for(int i = 0; i < 3; i++){ //idk bro
            spawnPos = player.transform.position + stepPos;
            foreach(ConfigurableJoint joint in joints){
                joint.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
                joint.transform.position = spawnPos;// + player.transform.position;
                spawnPos += stepPos;
            }
        }
    }
}
