using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMove : MonoBehaviour
{
    public float moveSpeed;
    public float maxChange;
    public float timeBetweenChanges;
    public float changeTimer;
    public Vector3 targetPos;
    public Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }


    private void Update()
    {
        changeTimer -= Time.deltaTime;
        if(changeTimer <= 0){
            changeTimer = timeBetweenChanges;
            targetPos = startPos + new Vector3(Random.Range(-maxChange, maxChange), Random.Range(-maxChange, maxChange), Random.Range(-maxChange, maxChange));
        }
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
