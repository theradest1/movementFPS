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
        int playerIndex = clientIDs.IndexOf(int.Parse(clientID));
        clientObjects[playerIndex].transform.position = parseVector3(position);
        clientObjects[playerIndex].transform.rotation = parseQuaternion(rotation);
        Debug.Log("Client's ID: " + clientID + "  New position: " + position + "  New rotation: " + rotation);
    }

    Vector3 parseVector3(string vector3String){
        vector3String = vector3String.Substring(1, vector3String.Length-2); //get rid of parenthisis
		string[] parts = vector3String.Split(',');
		return new Vector3(-float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
    }
    Quaternion parseQuaternion(string quaternionString){
        quaternionString = quaternionString.Substring(1, quaternionString.Length-2); //get rid of parenthisis
		string[] parts = quaternionString.Split(',');
		return new Quaternion(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
    }
}
