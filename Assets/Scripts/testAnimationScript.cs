using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAnimationScript : MonoBehaviour
{
    public Animator animator;
    public ControlsManager controlsManager;
    public float directionChangeSpeedVertical;
    public float directionChangeSpeedHorizontal;
    public Vector2 currentDirection;

    // Update is called once per frame
    void Update()
    {
        Vector2 moveDirection = controlsManager.moveDirection;
        currentDirection = new Vector2(Mathf.Lerp(currentDirection.x, moveDirection.x, Time.deltaTime * directionChangeSpeedHorizontal),  Mathf.Lerp(currentDirection.y, moveDirection.y, Time.deltaTime * directionChangeSpeedVertical));
        animator.SetFloat("x", currentDirection.x);
        animator.SetFloat("y", currentDirection.y);
    }
}
