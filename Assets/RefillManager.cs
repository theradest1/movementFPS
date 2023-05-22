using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillManager : MonoBehaviour
{
    public List<Refill> refills;
    int currentID = 0;
    public ServerEvents serverEvents;
    
    public GameObject toolRefillPrefab;
    public GameObject healthRefillPrefab;
    public GameObject throwableRefillPrefab;

    public void addRefill(string type, Vector3 position, Vector3 rotation, Vector3 velocity){
        GameObject refillPrefab = getRefillPrefab(type);
        GameObject refill = Instantiate(refillPrefab, position, Quaternion.Euler(rotation));
        refill.GetComponent<Rigidbody>().velocity = velocity;
        Refill newRefill = refill.GetComponent<Refill>();
        newRefill.ID = currentID;
        refills.Add(newRefill);
        currentID++;
    }

    GameObject getRefillPrefab(string type){
        if(type == "tool"){
            return toolRefillPrefab;
        }
        else if(type == "health"){
            return healthRefillPrefab;
        }
        else if(type == "throwable"){
            return throwableRefillPrefab;
        }
        return null;
    }

    public void collectedRefill(int collectedRefillID){
        foreach(Refill refill in refills){
            if(refill.ID == collectedRefillID){
                refills.Remove(refill);
                refill.clean();
                return;
            }
        }
    }

    public void sendNewRefillEvent(string type, Vector3 position, Vector3 rotation, Vector3 velocity){
        serverEvents.sendEvent("ue", "newrefill", type + "~" + position + "~" + rotation + "~" + velocity);
    }

    public void sendCollectEvent(int collectedRefillID){
        serverEvents.sendEvent("ue", "refill", collectedRefillID + "");
    }
}
