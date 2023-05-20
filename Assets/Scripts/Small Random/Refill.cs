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
                    GameObject.Find("manager").GetComponent<RefillManager>().sendCollectEvent(ID);
                }
            }
            else if(type == "health"){
                if(GameObject.Find("Player").GetComponent<PlayerManager>().collectHealth(healthOnCollect)){
                    GameObject.Find("manager").GetComponent<RefillManager>().sendCollectEvent(ID);
                }
            }
            else if(type == "throwable"){
                if(GameObject.Find("Player").GetComponent<Throwables>().collectThrowableCharge()){
                    GameObject.Find("manager").GetComponent<RefillManager>().sendCollectEvent(ID);
                }
            }
        }
    }

    public void clean(){
        Destroy(transform.parent.gameObject);
        Destroy(this.gameObject);
    }
}
