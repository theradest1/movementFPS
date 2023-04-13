using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAnimController : MonoBehaviour
{
    public Animator animator;

    public string reloadAnim;
    public float timeToReload;
    public string shootAnim;
    public float timeToShoot;
    public string pulloutAnim;
    public float timeToPullout;

    public float triggerReload(float speedMult){
        if(reloadAnim != ""){
            animator.SetFloat("reloadSpeed", 1/speedMult);
            animator.Play(reloadAnim);
            return timeToReload;
        }
        return 0;
    }
    public float triggerShoot(float speedMult){
        if(reloadAnim != ""){
            animator.SetFloat("shootSpeed", 1/speedMult);
            animator.Play(shootAnim);
            return timeToShoot;
        }
        return 0;
    }
    public float triggerPullout(float speedMult){
        if(reloadAnim != ""){
            animator.SetFloat("pulloutSpeed", 1/speedMult);
            animator.Play(pulloutAnim);
            return timeToPullout;
        }
        return 0;
    }
}
