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

    public void triggerReload(float scaledToTime){
        if(reloadAnim != ""){
            animator.SetFloat("reloadSpeed", timeToReload/scaledToTime);
            animator.Play(reloadAnim);
        }
    }
    public void triggerShoot(float scaledToTime){
        if(reloadAnim != ""){
            animator.SetFloat("shootSpeed", timeToShoot/scaledToTime);
            animator.Play(shootAnim);
        }
    }
    public void triggerPullout(float scaledToTime){
        if(reloadAnim != ""){
            animator.SetFloat("pulloutSpeed", timeToPullout/scaledToTime);
            animator.Play(pulloutAnim);
        }
    }
}
