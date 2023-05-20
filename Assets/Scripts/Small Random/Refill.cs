using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refill : MonoBehaviour
{
    public float spinSpeed;
    public string type;
    public int ID;
    public float timeBeforeDestroy = 30;

    [Header("Heal:")]
    public float healthOnCollect;
    [Header("Anim")]
    public float animTime = .03f;
    private void Start()
    {
        Invoke("clean", timeBeforeDestroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player"){
            //Debug.Log("Collected refill of type " + type);
            if(type == "tool"){
                if(GameObject.Find("Player").GetComponent<Tools>().collectCharge()){
                    StartCoroutine(collectAnim());
                }
            }
            else if(type == "health"){
                if(GameObject.Find("Player").GetComponent<PlayerManager>().collectHealth(healthOnCollect)){
                    StartCoroutine(collectAnim());
                }
            }
            else if(type == "throwable"){
                if(GameObject.Find("Player").GetComponent<Throwables>().collectThrowableCharge()){
                    StartCoroutine(collectAnim());
                }
            }
        }
    }

    public IEnumerator collectAnim(){
        //setup
        Destroy(transform.parent.GetComponent<Rigidbody>());
        Destroy(transform.parent.GetComponent<Collider>());
        Transform parentTransform = transform.parent.transform;
        Transform playerTransform = GameObject.Find("Player").transform;
        Vector3 startPos = transform.position;

        //anim
        float timer = Time.time;
        while(Time.time - timer <= animTime){
            parentTransform.position = Vector3.Lerp(startPos, playerTransform.position, (Time.time - timer)/animTime);
            yield return new WaitForEndOfFrame();
        }

        //trigger
        GameObject.Find("manager").GetComponent<RefillManager>().sendCollectEvent(ID);
    }

    public void clean(){
        Destroy(transform.parent.gameObject);
        Destroy(this.gameObject);
    }
}
