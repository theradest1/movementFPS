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

    public void triggerReload(float speedMult){
        if(reloadAnim != ""){
            animator.SetFloat("reloadSpeed", 1/speedMult);
            animator.Play(reloadAnim);
        }
    }
    public void triggerShoot(float speedMult){
        if(reloadAnim != ""){
            animator.SetFloat("shootSpeed", 1/speedMult);
            animator.Play(shootAnim);
        }
    }
    public void triggerPullout(float speedMult){
        if(reloadAnim != ""){
            animator.SetFloat("pulloutSpeed", 1/speedMult);
            animator.Play(pulloutAnim);
        }
    }
}
