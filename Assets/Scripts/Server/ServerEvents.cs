using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvents : MonoBehaviour
{

    public ServerComm serverComm;
    public void recieveEvent(string message){
        Debug.Log("Event recieved: " + message);
    }

    public void sendEvent(string eventType, string eventInfo){
        serverComm.send(eventType + "~" + eventInfo);
    }
}
