using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ServerEvents : MonoBehaviour
{
    //public List<GameObject> clientObjects;
    public List<OtherPlayer> clientScripts;
    public List<int> clientIDs;
    //public List<Vector3> targetPositions;
    //public List<Vector3> pastTargetPositions;
    //public List<Quaternion> targetRotations;
    //public List<Quaternion> pastTargetRotations;
    //public List<string> clientUsernames;
    public List<TextMeshProUGUI> scoreboardUsername;
    public List<TextMeshProUGUI> scoreboardKDRatio;
    //public List<int> kills;
    //public List<int> deaths;
    int clientKills = 0;
    int clientDeaths = 0;
    
    [HideInInspector]
    public float percentDone = 0;

    float startTime;

    public float invincibilityTimeOnDeath;

    ServerComm serverComm;
    PlayerManager playerManager;
    SoundManager soundManager;
    InGameGUIManager inGameGUIManager;
    ProjectileManager projectileManager;
    Weapons weapons;
    GameObject player;
    ReplayManager replayManager;
    ChatManager chatManager;
    Rigidbody playerRB;

    public bool replaying = false;
    public Quaternion playerTargetRot;
    public Quaternion playerPastTargetRot;
    public Vector3 playerTargetPos;
    public Vector3 playerPastTargetPos;

    public GameObject clientPrefab;
    public GameObject scoreboardLeftPrefab;
    public GameObject scoreboardLeftContainer;
    public GameObject scoreboardRightPrefab;
    public GameObject scoreboardRightContainer;

    TextMeshProUGUI clientUsernameScoreboard;
    TextMeshProUGUI clientKDScoreboard;

    private void Start() {

        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
        chatManager = GameObject.Find("manager").GetComponent<ChatManager>();
        replayManager = GameObject.Find("manager").GetComponent<ReplayManager>();
        weapons = GameObject.Find("Player").GetComponent<Weapons>();
        player = GameObject.Find("Player");
        inGameGUIManager = GameObject.Find("manager").GetComponent<InGameGUIManager>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        projectileManager = GameObject.Find("Player").GetComponent<ProjectileManager>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();

        clientUsernameScoreboard = Instantiate(scoreboardLeftPrefab, scoreboardLeftContainer.transform).GetComponent<TextMeshProUGUI>();
        clientKDScoreboard = Instantiate(scoreboardRightPrefab, scoreboardRightContainer.transform).GetComponent<TextMeshProUGUI>();

        clientUsernameScoreboard.text = " " + serverComm.username;

        playerTargetRot = Quaternion.identity;
        playerPastTargetRot = Quaternion.identity;
        playerTargetPos = Vector3.zero;
        playerPastTargetPos = Vector3.zero;
    }

    public void sendEvent(string eventType, string eventName, string eventInfo){
        if(eventName == "damage"){ //I'll eventually get rid of this but i'm currently lazy
            eventName = "d";
        }
        if(eventName == "sound" || eventName == "s"){ //I'll eventually get rid of this but i'm currently lazy
            eventName = "s";
            string[] imLazy = eventInfo.Split("~");
            playSound(imLazy[0], imLazy[1], imLazy[2], imLazy[3]);
        }
        string eventToSend = eventType + "~" + eventName + "~" + serverComm.ID + "~" + eventInfo;
        serverComm.send(eventToSend);
        //Debug.Log("Send event: " + eventToSend);
    }

    public void leave(){
        serverComm.send("leave~" + serverComm.ID);
    }

    public void sendEventFromOther(int senderID, string eventType, string eventName, string eventInfo){
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
        string killerUsername;
        string killedUsername;
        if(int.Parse(killedID) == serverComm.ID){
            clientDeaths += 1;
            clientKDScoreboard.text = clientKills + "/" + clientDeaths;
            playerManager.death(int.Parse(killerID));
            playerManager.changeHealth(-1000f);
            weapons.resetAll();
            killedUsername = serverComm.username;
        }
        else{
            OtherPlayer clientScript = clientScripts[clientIDs.IndexOf(int.Parse(killedID))];
            clientScript.changeScoreboard(0, 1);
            clientScript.invincibilityTimer = invincibilityTimeOnDeath;
            clientScript.changeHealth(-1000f); 
            killedUsername = clientScript.username;
            //scoreboardKDRatio[clientIDs.IndexOf(int.Parse(killedID))].text = kills[clientIDs.IndexOf(int.Parse(killedID))] + "/" + deaths[clientIDs.IndexOf(int.Parse(killedID))];
        }

        if(int.Parse(killerID) == serverComm.ID){
            clientKills += 1;
            clientKDScoreboard.text = clientKills + "/" + clientDeaths;
            killerUsername = serverComm.username;
        }
        else
        {
            OtherPlayer clientScript = getClientScript(killerID);
            Debug.Log(killerID);
            clientScript.changeScoreboard(1, 0);
            killerUsername = clientScript.username;
            //scoreboardKDRatio[clientIDs.IndexOf(int.Parse(killerID))].text = kills[clientIDs.IndexOf(int.Parse(killerID))] + "/" + deaths[clientIDs.IndexOf(int.Parse(killerID))];
        }
        inGameGUIManager.killFeed(killerUsername, killedUsername);

        //kills[clientIDs.IndexOf(int.Parse(killerID))] += 1;
        //scoreboardKDRatio[clientIDs.IndexOf(int.Parse(killerID))].text = kills[clientIDs.IndexOf(int.Parse(killerID))] + "/" + deaths[clientIDs.IndexOf(int.Parse(killerID))];
    }

    /*string getUsername(string _ID){ //is a string because thats how I recieve it from the server, not because I am dumb stupid (but I am, its just not corralated)
        if(int.Parse(_ID) == serverComm.ID){
            return serverComm.username;
        }
        else{
            return clientUsernames[clientIDs.IndexOf(int.Parse(_ID))];
        }
    }*/

    OtherPlayer getClientScript(string _ID){
        return clientScripts[clientIDs.IndexOf(int.Parse(_ID))];
    }

    public void newClient(string newClientID, string newClientUsername){
        //Debug.Log("New client's ID: " + newClientID + "  New client's username: " + newCleintUsername);
        chatManager.newChat(newClientUsername + " has joined the game", Color.red);
        GameObject newClientObject = Instantiate(clientPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
        newClientObject.name = newClientID;
        OtherPlayer clientScript = newClientObject.GetComponent<OtherPlayer>();
        clientScript.setUsername(newClientUsername);
        clientScripts.Add(newClientObject.GetComponent<OtherPlayer>());
        clientIDs.Add(int.Parse(newClientID));

        TextMeshProUGUI newClientScoreboardUsername = Instantiate(scoreboardLeftPrefab, scoreboardLeftContainer.transform).GetComponent<TextMeshProUGUI>();
        scoreboardUsername.Add(newClientScoreboardUsername);
        newClientScoreboardUsername.text = " "  + newClientUsername;
        TextMeshProUGUI newKDPeice = Instantiate(scoreboardRightPrefab, scoreboardRightContainer.transform).GetComponent<TextMeshProUGUI>();
        scoreboardKDRatio.Add(newKDPeice);
        clientScript.scoreboardPeice = newKDPeice;

        //Debug.Log("New player's ID: " + int.Parse(newClientID));
        sendEvent("ue", "setHealth", playerManager.health + "");
    }

    public void removeClient(string ID){
        //Debug.Log("Player with ID " + ID + " has left the game");
        int playerIndex = clientIDs.IndexOf(int.Parse(ID));
        chatManager.newChat(clientScripts[playerIndex].username + " has left the game", Color.red);
        clientIDs.RemoveAt(playerIndex);
        Destroy(clientScripts[playerIndex].gameObject);
        clientScripts.RemoveAt(playerIndex);

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

    public void setHealth(string clientID, string health){
        //Debug.Log("health: " + health + ");
        if(int.Parse(clientID) == serverComm.ID){
            playerManager.health = float.Parse(health);
        }
        else{
            int clientIDParsed = clientIDs.IndexOf(int.Parse(clientID));
            clientScripts[clientIDParsed].health = float.Parse(health);
            clientScripts[clientIDParsed].changeHealth(0f);
        }
    }

    public void clientDamage(string victimID, string damage){
        if(float.Parse(damage) > 0){
            if(int.Parse(victimID) != serverComm.ID){
                OtherPlayer clientScript = getClientScript(victimID);
                clientScript.changeHealth(0f);
            }
            else{
                playerManager.changeHealth(0f);
            }
        }
    }

    public void spawnProjectile(string senderID, string typeID, string damage, string position, string velocity){
        //projectileManager.createProjectile(int.Parse(senderID), int.Parse(typeID), float.Parse(damage), parseVector3(position), parseVector3(velocity));
        projectileManager.createProjectile(int.Parse(senderID), int.Parse(typeID), float.Parse(damage), parseVector3(position), Quaternion.identity, parseVector3(velocity), Vector3.zero);
    }

    public void playSound(string clipID, string position, string volume, string pitch){
        soundManager.playSound(int.Parse(clipID), parseVector3(position), float.Parse(volume), float.Parse(pitch));
    }

    public void update(string clientID, string position, string rotation, bool isReplayData){
        if(int.Parse(clientID) != serverComm.ID && ((!replaying && !isReplayData) || (replaying && isReplayData))){
            /*if(replaying){
                Debug.Log("Updated client with ID " + clientID + ", is replay data: " + isReplayData + ", pos: " + position + ", rot: " + rotation);
            }*/
            OtherPlayer clientScript = getClientScript(clientID);
            //Debug.Log(clientID);
            clientScript.direction = clientScript.targetPosition - parseVector3(position);
            clientScript.pastTargetPosition = clientScript.targetPosition;
            clientScript.targetPosition = parseVector3(position);
            clientScript.pastTargetRotation = clientScript.targetRotation;
            clientScript.targetRotation = parseQuaternion(rotation);
            //Debug.Log("Client's ID: " + clientID + "  New position: " + position + "  New rotation: " + rotation);
        }
    }

    public Vector3 parseVector3(string vector3String){
        vector3String = vector3String.Substring(1, vector3String.Length-2); //get rid of parenthisis
		string[] parts = vector3String.Split(',');
		return new Vector3(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
    }
    public Quaternion parseQuaternion(string quaternionString){
        quaternionString = quaternionString.Substring(1, quaternionString.Length-2); //get rid of parenthisis
		string[] parts = quaternionString.Split(',');
		return new Quaternion(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
    }

    public void resetSmoothTimer(){
        startTime = Time.time;
    }

    void Update(){
        if(!replaying){
            percentDone = (Time.time - startTime) / serverComm.updateSpeed;
        }
        else{
            percentDone = (Time.time - startTime) / (1f/(float) (replayManager.tickRate / replayManager.replaySlowdown));
            playerRB.position = Vector3.Lerp(playerPastTargetPos, playerTargetPos, percentDone);
            playerRB.rotation = Quaternion.Slerp(playerPastTargetRot, playerTargetRot, percentDone);
        }
    }
}
