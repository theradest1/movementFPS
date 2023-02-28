using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerEvents : MonoBehaviour
{
    public List<GameObject> clientObjects;
    public List<OtherPlayer> clientScripts;
    public List<int> clientIDs;
    public List<Vector3> targetPositions;
    public List<Vector3> pastTargetPositions;
    public List<string> clientUsernames;
    public List<TextMeshProUGUI> scoreboardUsername;
    public List<TextMeshProUGUI> scoreboardKDRatio;
    public List<int> kills;
    public List<int> deaths;
    int clientKills = 0;
    int clientDeaths = 0;

    float startTime;

    public float invincibilityTimeOnDeath;

    ServerComm serverComm;
    PlayerManager playerManager;
    SoundManager soundManager;
    InGameGUIManager inGameGUIManager;
    ProjectileManager projectileManager;
    WeaponManager weaponManager;
    GameObject player;


    public GameObject clientPrefab;
    public GameObject scoreboardLeftPrefab;
    public GameObject scoreboardLeftContainer;
    public GameObject scoreboardRightPrefab;
    public GameObject scoreboardRightContainer;

    TextMeshProUGUI clientUsernameScoreboard;
    TextMeshProUGUI clientKDScoreboard;

    private void Start() {

        weaponManager = GameObject.Find("Player").GetComponent<WeaponManager>();
        player = GameObject.Find("Player");
        inGameGUIManager = GameObject.Find("manager").GetComponent<InGameGUIManager>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        projectileManager = GameObject.Find("Player").GetComponent<ProjectileManager>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        clientUsernameScoreboard = Instantiate(scoreboardLeftPrefab, scoreboardLeftContainer.transform).GetComponent<TextMeshProUGUI>();
        clientKDScoreboard = Instantiate(scoreboardRightPrefab, scoreboardRightContainer.transform).GetComponent<TextMeshProUGUI>();

        clientUsernameScoreboard.text = " " + serverComm.username;
    }

    public void sendEvent(string eventType, string eventName, string eventInfo){
        if(eventType == "universalEvent"){ //I'll eventually get rid of this but i'm currently lazy
            eventType = "ue";
        }
        if(eventName == "damage"){ //I'll eventually get rid of this but i'm currently lazy
            eventName = "d";
        }
        if(eventName == "sound"){ //I'll eventually get rid of this but i'm currently lazy
            eventName = "s";
        }
        string eventToSend = eventType + "~" + eventName + "~" + serverComm.ID + "~" + eventInfo;
        serverComm.send(eventToSend);
        //Debug.Log("Send event: " + eventToSend);
    }

    public void sendEventFromOther(int senderID, string eventType, string eventName, string eventInfo){
        if(eventType == "universalEvent"){ //I'll eventually get rid of this but i'm currently lazy
            eventType = "ue";
        }
        if(eventName == "damage"){ //I'll eventually get rid of this but i'm currently lazy
            eventName = "d";
        }
        if(eventName == "sound"){ //I'll eventually get rid of this but i'm currently lazy
            eventName = "s";
        }
        string eventToSend = eventType + "~" + eventName + "~" + senderID + "~" + eventInfo;
        serverComm.send(eventToSend);
        //Debug.Log("Send event: " + eventToSend);
    }

    public void death(string killerID, string killedID){
        if(int.Parse(killedID) == serverComm.ID){
            clientDeaths += 1;
            clientKDScoreboard.text = clientKills + "/" + clientDeaths;
        }
        else
        {
            deaths[clientIDs.IndexOf(int.Parse(killedID))] += 1;    
            scoreboardKDRatio[clientIDs.IndexOf(int.Parse(killedID))].text = kills[clientIDs.IndexOf(int.Parse(killedID))] + "/" + deaths[clientIDs.IndexOf(int.Parse(killedID))];
        }

        if(int.Parse(killerID) == serverComm.ID){
            clientKills += 1;
            clientKDScoreboard.text = clientKills + "/" + clientDeaths;
        }
        else
        {
            kills[clientIDs.IndexOf(int.Parse(killerID))] += 1;    
            scoreboardKDRatio[clientIDs.IndexOf(int.Parse(killerID))].text = kills[clientIDs.IndexOf(int.Parse(killerID))] + "/" + deaths[clientIDs.IndexOf(int.Parse(killerID))];
        }

        //kills[clientIDs.IndexOf(int.Parse(killerID))] += 1;
        //scoreboardKDRatio[clientIDs.IndexOf(int.Parse(killerID))].text = kills[clientIDs.IndexOf(int.Parse(killerID))] + "/" + deaths[clientIDs.IndexOf(int.Parse(killerID))];
        inGameGUIManager.killFeed(getUsername(killerID), getUsername(killedID));
        if(int.Parse(killedID) == serverComm.ID){
            playerManager.spawn();// player.transform.position = new Vector3(Random.Range(0, 28), 14f, Random.Range(0, 28));
            playerManager.changeHealth(-1000f);
            weaponManager.resetAllWeapons();
        }
        else{
            clientScripts[clientIDs.IndexOf(int.Parse(killedID))].invincibilityTimer = invincibilityTimeOnDeath;
            clientScripts[clientIDs.IndexOf(int.Parse(killedID))].changeHealth(-1000f); 
        }
    }

    string getUsername(string _ID){ //is a string because thats how I recieve it from the server, not because I am dumb stupid (but I am, its just not corralated)
        if(int.Parse(_ID) == serverComm.ID){
            return serverComm.username;
        }
        else{
            return clientUsernames[clientIDs.IndexOf(int.Parse(_ID))];
        }
    }

    public void newClient(string newClientID, string newCleintUsername){
        //Debug.Log("New client's ID: " + newClientID + "  New client's username: " + newCleintUsername);
        GameObject newClientObject = Instantiate(clientPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        newClientObject.name = newClientID;
        newClientObject.GetComponent<OtherPlayer>().setUsername(newCleintUsername);
        clientObjects.Add(newClientObject);
        clientScripts.Add(newClientObject.GetComponent<OtherPlayer>());
        clientUsernames.Add(newCleintUsername);
        clientIDs.Add(int.Parse(newClientID));
        deaths.Add(0);
        kills.Add(0);
        TextMeshProUGUI newClientScoreboardUsername = Instantiate(scoreboardLeftPrefab, scoreboardLeftContainer.transform).GetComponent<TextMeshProUGUI>();
        scoreboardUsername.Add(newClientScoreboardUsername);
        newClientScoreboardUsername.text = " "  + newCleintUsername;
        scoreboardKDRatio.Add(Instantiate(scoreboardRightPrefab, scoreboardRightContainer.transform).GetComponent<TextMeshProUGUI>());

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
        clientUsernames.RemoveAt(playerIndex);

        Destroy(scoreboardKDRatio[playerIndex].gameObject);
        Destroy(scoreboardUsername[playerIndex].gameObject);
        scoreboardKDRatio.RemoveAt(playerIndex);
        scoreboardUsername.RemoveAt(playerIndex);
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

    public void spawnProjectile(string senderID, string typeID, string damage, string position, string velocity){
        //Debug.Log(senderID + ", " + typeID + ", " + damage + ", " + position + ", " + velocity);
        projectileManager.createProjectile(int.Parse(senderID), int.Parse(typeID), float.Parse(damage), parseVector3(position), parseVector3(velocity));
        //GameObject newProjectile = Instantiate(projectilePrefabs[int.Parse(typeID)], parseVector3(position), Quaternion.identity);
        //newProjectile.GetComponent<Rigidbody>().velocity = parseVector3(velocity);
    }

    /*public void spawnFlash(string position, string velocity){
        GameObject newFlash = Instantiate(flashPrefab, parseVector3(position), Quaternion.identity);
        newFlash.GetComponent<Rigidbody>().velocity = parseVector3(velocity);
        //Debug.Log("flash");
    }

    public void spawnGranade(string position, string velocity){
        GameObject newGranade = Instantiate(granadePrefab, parseVector3(position), Quaternion.identity);
        newGranade.GetComponent<Rigidbody>().velocity = parseVector3(velocity);
        //Debug.Log("flash");
    }*/

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
