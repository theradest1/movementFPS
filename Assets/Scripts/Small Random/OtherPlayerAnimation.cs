    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayerAnimation : MonoBehaviour
{
    public Animator animator;
    public OtherPlayer otherPlayer;
    public float directionChangeSpeedVertical;
    public float directionChangeSpeedHorizontal;
    public Vector2 direction;
    public float speedToAnimMult;
    public float maxSpeed;
    LayerMask groundMask;

    private void Start() {
        groundMask = GameObject.Find("Player").GetComponent<movement>().groundMask;
    }

    void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.6f, 0f), .45f);
    }

    private void Update() {
        if(Physics.CheckSphere(transform.position + new Vector3(0f, -.6f, 0f), .45f, groundMask)){
            direction = new Vector2(Mathf.Lerp(direction.x, otherPlayer.direction.x * speedToAnimMult, Time.deltaTime * directionChangeSpeedHorizontal),  Mathf.Lerp(direction.y, otherPlayer.direction.z * speedToAnimMult, Time.deltaTime * directionChangeSpeedVertical));
        }
        else{
            direction = new Vector2(Mathf.Lerp(direction.x, 0f, Time.deltaTime * directionChangeSpeedHorizontal),  Mathf.Lerp(direction.y, 0f, Time.deltaTime * directionChangeSpeedVertical));
        }
        Vector2 actualDirection = rotate(direction, transform.eulerAngles.y);
        animator.SetFloat("x", actualDirection.x);
        animator.SetFloat("y", actualDirection.y);
        if(actualDirection.magnitude > maxSpeed){
            maxSpeed = actualDirection.magnitude;
        }
    }

    public static Vector2 rotate(Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta), v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta));
    }

}
