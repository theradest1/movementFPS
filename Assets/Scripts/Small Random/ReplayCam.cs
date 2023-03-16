using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCam : MonoBehaviour
{
    public GameObject boomArm;
    public GameObject player;
    public GameObject otherObject;
    public float minDist;
    public float maxDist;
    public float smoothness;
    public float minBoomArmRotSpeed;
    public float distance;
    public LayerMask stopFrom;
    public Camera replayCamera;
    public float otherObjectMinPos;

    private void Update() {
        
        RaycastHit hit;
        if(Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, Vector3.Distance(transform.position, player.transform.position), stopFrom)){
            distance = Mathf.Clamp(Vector3.Distance(player.transform.position, hit.point), minDist, maxDist);
        }
        else{
            distance = Mathf.Clamp(distance + smoothness * Time.deltaTime, minDist, maxDist);
        }
        transform.position = player.transform.position + Vector3.Normalize(transform.position - player.transform.position) * distance;
        //boomArm.transform.Rotate(0f, (maxDist - distance) * minBoomArmRotSpeed * Time.deltaTime, 0f, Space.World);
        transform.LookAt(player.transform.position);

        Vector2 otherObjectPos = new Vector2(replayCamera.WorldToScreenPoint(otherObject.transform.position).x/Screen.width, replayCamera.WorldToScreenPoint(otherObject.transform.position).y/Screen.height);
        if(otherObjectPos.x > otherObjectMinPos){
            boomArm.transform.Rotate(0f, smoothness * Time.deltaTime, 0f, Space.World);
        }
        else if(otherObjectPos.x < 1 - otherObjectMinPos){
            boomArm.transform.Rotate(0f, -smoothness * Time.deltaTime, 0f, Space.World);
        }
        if(otherObjectPos.y > otherObjectMinPos){
            boomArm.transform.Rotate(-smoothness * Time.deltaTime, 0f, 0f, Space.World);
        }
        else if(otherObjectPos.y < 1 - otherObjectMinPos){
            boomArm.transform.Rotate(smoothness * Time.deltaTime, 0f, 0f, Space.World);
        }
    }
}
