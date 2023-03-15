using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    [HideInInspector]
    public List<string> playerReplayData = new List<string>();
    [Header("References:")]
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public PlayerManager playerManager;
    public Look look;
    public InGameGUIManager inGameGUIManager;
    public GameObject player;

    [Header("Settings:")]
    public int tickRate = 10;
    public int replayTime = 3; 
    public float replaySlowdown = 10;

    int currentTick = -1;

    private void Start() {
        for(int i = 0; i < tickRate * replayTime; i++){
            playerReplayData.Add("");
        }
        InvokeRepeating("storeReplay", 0, 1f/(float)tickRate);
    }

    void storeReplay(){
        if(!serverEvents.replaying){
            currentTick++;
            if(currentTick >= tickRate*replayTime){
                currentTick = 0;
            }
            playerReplayData[currentTick] = serverComm.ID + "~" + player.transform.position + "~" + player.transform.eulerAngles;
            MainListToString();
        }
    }

    public IEnumerator startReplay(List<List<string>> replayData){
        Debug.Log("started replay");
        playerManager.deathMenu.SetActive(false);
        int playerIndex;
        string[] individualPlayerTickData;
        serverEvents.replaying = true;
        look.camRotX = 0f;
        int startingTick = currentTick;
        int tick = startingTick + 1;
        while(tick != startingTick){
            foreach(List<string> individualPlayerData in replayData){
                if(individualPlayerData.Count > 0){
                    individualPlayerTickData = individualPlayerData[tick].Split("~");
                    //Debug.Log("updating player with ID " + individualPlayerTickData[0]);
                    if(individualPlayerData[tick] != ""){
                        //Debug.Log("AWDAWDAWDAW");
                        if(int.Parse(individualPlayerTickData[0]) != serverComm.ID && int.Parse(individualPlayerTickData[0]) != -1){
                            playerIndex = serverEvents.clientIDs.IndexOf(int.Parse(individualPlayerTickData[0]));
                            
                            serverEvents.pastTargetPositions[playerIndex] = serverEvents.targetPositions[playerIndex];
                            serverEvents.targetPositions[playerIndex] = serverEvents.parseVector3(individualPlayerTickData[1]);
                            serverEvents.pastTargetRotations[playerIndex] = serverEvents.targetRotations[playerIndex];
                            serverEvents.targetRotations[playerIndex] = Quaternion.Euler(serverEvents.parseVector3(individualPlayerTickData[2]));
                        }
                        else{
                            serverEvents.playerPastTargetPos = serverEvents.playerTargetPos;
                            serverEvents.playerTargetPos = serverEvents.parseVector3(individualPlayerTickData[1]);
                            serverEvents.playerPastTargetRot = serverEvents.playerTargetRot;
                            serverEvents.playerTargetRot = Quaternion.Euler(serverEvents.parseVector3(individualPlayerTickData[2]));
                        }
                    }
                    else{
                        tick = startingTick - 1;
                    }
                }
            }
            tick++;
            if(tick >= tickRate*replayTime){
                tick = 0;
            }
            serverEvents.resetSmoothTimer();
            yield return new WaitForSeconds(1f/((float)tickRate / replaySlowdown));
        }
        playerManager.deathMenu.SetActive(true);
        serverEvents.replaying = false;
        Debug.Log("ended replay");
    }

    private void MainListToString() {
        string thingToPrint = "";
        foreach(OtherPlayer otherPlayer in serverEvents.clientScripts){
            thingToPrint = "";
            foreach(string tick in otherPlayer.replayData){
                thingToPrint += tick + "~";
            }
            //Debug.Log("Client with ID " + otherPlayer.gameObject.name + ": " + thingToPrint);
        }

        thingToPrint = "";
        foreach(string tick in playerReplayData){
            thingToPrint += tick + "~";
        }
        //Debug.Log("Self: " + thingToPrint);
        /*string final = "[";
        foreach(List<string> listElement in list){
            final += "[";
            foreach(string tickListElement in listElement){
                final += tickListElement + ", ";
            }
            final += "]";
        }
        return final + "]";*/
    }
}