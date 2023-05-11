using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayCam : MonoBehaviour
{
    /*public GameObject boomArm;
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
    public float maxXRot;
    public float preferredXRot;
    public float preferredSpeed;
    public float minXRot;*/
    [HideInInspector]
    public Vector3 targetPos;

    public IEnumerator panToPos(Vector3 startPos, Vector3 endPos, float totalTime){
        float timer = Time.time;
        while(Time.time - timer <= totalTime){
            transform.LookAt(endPos);
            transform.position = Vector3.Lerp(startPos, endPos, (Time.time - timer)/totalTime);
            yield return 0;
        }
        yield return null;
    }

    public IEnumerator follow(GameObject objectToFollow, float totalTime, float distance){
        float timer = Time.time;
        while(Time.time - timer <= totalTime){
            transform.position = objectToFollow.transform.position - objectToFollow.transform.forward * distance + Vector3.up * distance;
            transform.rotation = objectToFollow.transform.rotation;
            yield return 0;
        }
        yield return null;
    }

    private void Update() {
        //transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
        /*
        boomArm.transform.position = player.transform.position;
        
        RaycastHit hit;
        Physics.Raycast(player.transform.position, transform.position - player.transform.position, out hit, maxDist, stopFrom);
        distance = Mathf.Clamp(Vector3.Distance(player.transform.position, hit.point), minDist, maxDist);

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
        else{
            boomArm.transform.Rotate(-(boomArm.transform.eulerAngles.x/Mathf.Abs(boomArm.transform.eulerAngles.x)) * Time.deltaTime / preferredSpeed, 0f, 0f, Space.World);
        }
        boomArm.transform.eulerAngles = new Vector3(Mathf.Clamp(boomArm.transform.eulerAngles.x, minXRot, maxXRot), boomArm.transform.eulerAngles.y, boomArm.transform.eulerAngles.z);
        */
    }
}