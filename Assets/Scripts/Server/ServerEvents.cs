using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvents : MonoBehaviour
{
    public List<GameObject> clientObjects;
    public List<int> clientIDs;
    public List<Vector3> targetPositions;
    public List<Vector3> pastTargetPositions;
    public ServerComm serverComm;
    public GameObject clientPrefab;
    float startTime;

    public void recieveEvent(string message){
        Debug.Log("Event recieved: " + message);
    }

    public void sendEvent(string eventType, string eventInfo){
        serverComm.send(eventType + "~" + eventInfo);
    }

    public void newClient(string newClientID, string newCleintUsername){
        //Debug.Log("New client's ID: " + newClientID + "  New client's username: " + newCleintUsername);
        GameObject newClientObject = Instantiate(clientPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        newClientObject.name = newClientID;
        clientObjects.Add(newClientObject);
        clientIDs.Add(int.Parse(newClientID));
        Debug.Log("New player's ID: " + int.Parse(newClientID));
        pastTargetPositions.Add(new Vector3(0f, 0f, 0f));
        targetPositions.Add(new Vector3(0f, 0f, 0f));
    }

    public void removeClient(string ID){
        Debug.Log("Player with ID " + ID + " has left the game");
        int playerIndex = clientIDs.IndexOf(int.Parse(ID));
        clientIDs.RemoveAt(playerIndex);
        pastTargetPositions.RemoveAt(playerIndex);
        targetPositions.RemoveAt(playerIndex);
        Destroy(clientObjects[playerIndex]);
        clientObjects.RemoveAt(playerIndex);
    }

    public void update(string clientID, string position, string rotation){
        if(int.Parse(clientID) != serverComm.ID){
            int playerIndex = clientIDs.IndexOf(int.Parse(clientID));
            Debug.Log(clientID);
            pastTargetPositions[playerIndex] = targetPositions[playerIndex];
            targetPositions[playerIndex] = parseVector3(position);
            clientObjects[playerIndex].transform.rotation = parseQuaternion(rotation);
            //Debug.Log("Client's ID: " + clientID + "  New position: " + position + "  New rotation: " + rotation);
        }
    }

    Vector3 parseVector3(string vector3String){
        vector3String = vector3String.Substring(1, vector3String.Length-2); //get rid of parenthisis
		string[] parts = vector3String.Split(',');
		return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
    }
    Quaternion parseQuaternion(string quaternionString){
        quaternionString = quaternionString.Substring(1, quaternionString.Length-2); //get rid of parenthisis
		string[] parts = quaternionString.Split(',');
		return new Quaternion(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
    }

    public void resetSmoothTimer(){
        startTime = Time.time;
    }

    void Update(){
        float percentDone = (Time.time - startTime) / serverComm.updateSpeed;
        //Debug.Log("Percent: " + percentDone);
        //Debug.Log("Time Difference: " + (Time.time - startTime));
        //Debug.Log("Start timer: " + startTime);

        for(int i = 0; i < clientObjects.Count; i++){
            clientObjects[i].transform.position = Vector3.Lerp(pastTargetPositions[i], targetPositions[i], percentDone);
            //Debug.Log("Past pos: " + pastTargetPositions[i] + "  CurrentTargetPos: " + targetPositions[i]);
        }
    }
}
