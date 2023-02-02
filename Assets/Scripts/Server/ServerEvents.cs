using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvents : MonoBehaviour
{
    public List<GameObject> clientObjects;
    public List<int> clientIDs;
    public ServerComm serverComm;
    public GameObject clientPrefab;

    public void recieveEvent(string message){
        Debug.Log("Event recieved: " + message);
    }

    public void sendEvent(string eventType, string eventInfo){
        serverComm.send(eventType + "~" + eventInfo);
    }

    public void newClient(string newClientID, string newCleintUsername){
        Debug.Log("New client's ID: " + newClientID + "  New client's username: " + newCleintUsername);
        GameObject newClientObject = Instantiate(clientPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        newClientObject.name = newClientID;
        clientObjects.Add(newClientObject);
        clientIDs.Add(int.Parse(newClientID));
    }

    public void update(string clientID, string position, string rotation){
        Debug.Log("Client's ID: " + clientID + "  New position: " + position + "  New rotation: " + rotation);
    }
}
