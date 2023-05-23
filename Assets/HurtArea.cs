using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtArea : MonoBehaviour
{
    public float damage;
    public int userID;
    ProjectileFunctions projectileFunctions;

    public float timePerTick;
    public float tickTimer;
    public float particleTimeToStop;
    public ParticleSystem particles;
    public bool useCollider;
    public float distance;

    private void Start()
    {
        projectileFunctions = GameObject.Find("manager").GetComponent<ProjectileFunctions>();
    }

    private void Update()
    {
        tickTimer -= Time.deltaTime;
        if(!useCollider && Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position) <= distance){
            if(tickTimer <= 0){
                Debug.Log("Hurting with distance");
                projectileFunctions.triggerDamage(null, damage * timePerTick, userID);
                tickTimer = timePerTick;
            }
        }
        else{
            Debug.Log(Vector3.Distance(transform.position, projectileFunctions.playerCam.transform.position));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(useCollider){
            if(tickTimer <= 0){
                Debug.Log("Hurting");
                projectileFunctions.triggerDamage(null, damage * timePerTick, userID);
                tickTimer = timePerTick;
            }
        }
    }

    public void slowStop(float timeToDestroy){
        Invoke("stopParticles", timeToDestroy);
        Destroy(this.gameObject, timeToDestroy + particleTimeToStop);
    }

    public void stopParticles(){
        particles.Stop();
    }

}
