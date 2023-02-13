using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerEvents : MonoBehaviour
{
    public List<GameObject> clientObjects;
    public List<OtherPlayer> clientScripts;
    public List<int> clientIDs;
    public List<Vector3> targetPositions;
    public List<Vector3> pastTargetPositions;
    public ServerComm serverComm;
    public GameObject clientPrefab;
    public PlayerManager playerManager;
    float startTime;
    public GameObject bulletPrefab;
    public SoundManager soundManager;
    public GameObject flashPrefab;

    public void sendEvent(string eventType, string eventName, string eventInfo){
        string eventToSend = eventType + "~" + eventName + "~" + serverComm.ID + "~" + eventInfo;
        serverComm.send(eventToSend);
        //Debug.Log("Send event: " + eventToSend);
    }

    public void newClient(string newClientID, string newCleintUsername){
        //Debug.Log("New client's ID: " + newClientID + "  New client's username: " + newCleintUsername);
        GameObject newClientObject = Instantiate(clientPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        newClientObject.name = newClientID;
        newClientObject.GetComponent<OtherPlayer>().setUsername(newCleintUsername);
        clientObjects.Add(newClientObject);
        clientScripts.Add(newClientObject.GetComponent<OtherPlayer>());
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
        clientScripts.RemoveAt(playerIndex);
    }

    public void damage(string attackerID, string victimID, string damage){
        //Debug.Log("Player with ID " + attackerID + " attacked player with ID " + victimID + " for " + damage + " damage.");
        if(int.Parse(victimID) != serverComm.ID){
            int victimIndex = clientIDs.IndexOf(int.Parse(victimID));
            clientScripts[victimIndex].changeHealth(float.Parse(damage));
        }
        else{
            playerManager.changeHealth(float.Parse(damage));
        }
    }

    public void spawnBullet(string senderID, string position, string rotation, string travelSpeed){
        GameObject bullet = Instantiate(bulletPrefab, parseVector3(position), parseQuaternion(rotation));
        bullet.GetComponent<BulletScript>().goTo(float.Parse(travelSpeed), this, 0f, false, null);
    }

    public void playSound(string clipID, string position, string volume, string pitch){
        soundManager.playSound(int.Parse(clipID), parseVector3(position), float.Parse(volume), float.Parse(pitch));
    }

    public void update(string clientID, string position, string rotation){
        if(int.Parse(clientID) != serverComm.ID){
            int playerIndex = clientIDs.IndexOf(int.Parse(clientID));
            //Debug.Log(clientID);
            pastTargetPositions[playerIndex] = targetPositions[playerIndex];
            targetPositions[playerIndex] = parseVector3(position);
            clientObjects[playerIndex].transform.rotation = parseQuaternion(rotation);
            //Debug.Log("Client's ID: " + clientID + "  New position: " + position + "  New rotation: " + rotation);
        }
    }

    public void spawnFlash(string position, string velocity){
        GameObject newFlash = Instantiate(flashPrefab, parseVector3(position), Quaternion.identity);
        newFlash.GetComponent<Rigidbody>().velocity = parseVector3(velocity);
        //Debug.Log("flash");
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

        for(int i = 0; i < clientObjects.Count; i++){
            clientObjects[i].transform.position = Vector3.Lerp(pastTargetPositions[i], targetPositions[i], percentDone);
        }
    }
}
