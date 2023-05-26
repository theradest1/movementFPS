using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refill : MonoBehaviour
{
    public float spinSpeed;
    public string type;
    public int ID;
    public float timeBeforeDestroy = 30;
    public float distanceToCollect = 2.5f;
    bool active = true;

    [Header("Heal:")]
    public float healthOnCollect;
    [Header("Anim")]
    public float animTime = .03f;

    //player references
    Transform player;
    PlayerManager playerManager;
    Tools tools;
    Throwables throwables;

    private void Start()
    {
        player = GameObject.Find("Player").transform;
        tools = GameObject.Find("Player").GetComponent<Tools>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        throwables = GameObject.Find("Player").GetComponent<Throwables>();
        Invoke("clean", timeBeforeDestroy);
    }

    private void Update() {
        if(active){
            if(Vector3.Distance(transform.position, player.position) <= distanceToCollect){
                if(type == "tool"){
                    if(tools.collectCharge()){
                        StartCoroutine(collectAnim());
                    }
                }
                else if(type == "health"){
                    if(playerManager.collectHealth(healthOnCollect)){
                        StartCoroutine(collectAnim());
                    }
                }
                else if(type == "throwable"){
                    if(throwables.collectThrowableCharge()){
                        StartCoroutine(collectAnim());
                    }
                }
            }
        }
    }

    public IEnumerator collectAnim(){
        //setup
        active = false;
        Transform playerTransform = GameObject.Find("Player").transform;
        Vector3 startPos = transform.position;

        //anim
        float timer = Time.time;
        while(Time.time - timer <= animTime){
            transform.position = Vector3.Lerp(startPos, playerTransform.position + Vector3.up/3, (Time.time - timer)/animTime);
            yield return new WaitForEndOfFrame();
        }

        //trigger
        GameObject.Find("manager").GetComponent<RefillManager>().sendCollectEvent(ID);
    }

    public void clean(){
        Destroy(this.gameObject);
    }
}
