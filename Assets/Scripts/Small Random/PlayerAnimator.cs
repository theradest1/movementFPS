using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    public Animator animator;
    public float directionChangeSpeedVertical;
    public float directionChangeSpeedHorizontal;
    public Vector2 direction;
    public float speedToAnimMult;
    LayerMask groundMask;
    movement movementScript;
    public Rigidbody rb;

    private void Start() {
        movementScript = GameObject.Find("Player").GetComponent<movement>();
        groundMask = GameObject.Find("Player").GetComponent<movement>().groundMask;
    }

    /*void OnDrawGizmos(){
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + new Vector3(0f, -.6f, 0f), .45f);
    }*/

    private void Update() {
        if(movementScript.isGrounded){
            direction = new Vector2(Mathf.Lerp(direction.x, rb.velocity.x * speedToAnimMult, Time.deltaTime * directionChangeSpeedHorizontal),  Mathf.Lerp(direction.y, rb.velocity.z * speedToAnimMult, Time.deltaTime * directionChangeSpeedVertical));
        }
        else{
            direction = new Vector2(Mathf.Lerp(direction.x, 0f, Time.deltaTime * directionChangeSpeedHorizontal),  Mathf.Lerp(direction.y, 0f, Time.deltaTime * directionChangeSpeedVertical));
        }
        Vector2 actualDirection = rotate(direction, transform.eulerAngles.y);
        animator.SetFloat("x", actualDirection.x);
        animator.SetFloat("y", actualDirection.y);
    }

    public static Vector2 rotate(Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta), v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta));
    }

}
