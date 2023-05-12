using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayManager : MonoBehaviour
{
    [HideInInspector]
    public List<string> playerReplayData = new List<string>();
    public bool enter = false;

    [Header("References:")]
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public PlayerManager playerManager;
    public ControlsManager controlsManager;
    public Look look;
    public InGameGUIManager inGameGUIManager;
    public GameObject player;
    public ReplayCam replayCam;
    public GameObject cam;
    public GameObject fouxBullet;

    [Header("Settings:")]
    public int tickRate = 10;
    public int replayTime = 3; 
    public float replaySlowdown = 1;
    public float bulletTimeToTravel = .1f;
    public float bulletToGunTime = .1f;

    GameObject currentOtherObject;

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
            //MainListToString();
        }
        foreach(OtherPlayer otherPlayer in serverEvents.clientScripts){
            otherPlayer.storeReplayData(currentTick);
        }
    }

    void cameraPan(){
        fouxBullet.SetActive(true);
        StartCoroutine(replayCam.panToPos(currentOtherObject.transform.position + Vector3.up, player.transform.position + Vector3.up, bulletTimeToTravel));
    }

    void cameraMoveToGun(){
        StartCoroutine(replayCam.panToPos(replayCam.transform.position, currentOtherObject.transform.position + Vector3.up, bulletToGunTime));
    }

    public IEnumerator startReplay(List<List<string>> replayData, GameObject otherObject){
        //Debug.Log("started replay:");
        currentOtherObject = otherObject;
        enter = false;
        serverEvents.replaying = true;
        cam.SetActive(false);
        replayCam.gameObject.SetActive(true);
        StartCoroutine(replayCam.follow(currentOtherObject, replayTime * replaySlowdown - bulletTimeToTravel - bulletToGunTime, 2));
        Invoke("cameraMoveToGun", replayTime * replaySlowdown - bulletTimeToTravel - bulletToGunTime);
        Invoke("cameraPan", replayTime * replaySlowdown - bulletTimeToTravel);
        //replayCam.distance = replayCam.maxDist;
        //Debug.Log(MainListToString(replayData));
        playerManager.deathMenu.SetActive(false);
        //int playerIndex;
        string[] individualPlayerTickData;
        look.camRotX = 0f;
        int startingTick = currentTick;
        int tick = startingTick + 1;
        while(tick != startingTick && tick < tickRate*replayTime + 5){ //the second statement is to stop a softlock
            foreach(List<string> individualPlayerData in replayData){
                if(individualPlayerData.Count > 0){
                    individualPlayerTickData = individualPlayerData[tick].Split("~");
                    if(individualPlayerData[tick] != ""){
                        //Debug.Log("AWDAWDAWDAW");
                        if(int.Parse(individualPlayerTickData[0]) != serverComm.ID && int.Parse(individualPlayerTickData[0]) != -1){
                            //Debug.Log("New data: " + individualPlayerData[tick]);
                            serverEvents.update(individualPlayerTickData[0], individualPlayerTickData[1], Quaternion.Euler(serverEvents.parseVector3(individualPlayerTickData[2])) + "", true);
                            /*playerIndex = serverEvents.clientIDs.IndexOf(int.Parse(individualPlayerTickData[0]));
                            
                            serverEvents.pastTargetPositions[playerIndex] = serverEvents.targetPositions[playerIndex];
                            serverEvents.targetPositions[playerIndex] = serverEvents.parseVector3(individualPlayerTickData[1]);
                            serverEvents.pastTargetRotations[playerIndex] = serverEvents.targetRotations[playerIndex];
                            serverEvents.targetRotations[playerIndex] = Quaternion.Euler(serverEvents.parseVector3(individualPlayerTickData[2]));
                            */
                        }
                        else{
                            serverEvents.playerPastTargetPos = serverEvents.playerTargetPos;
                            serverEvents.playerTargetPos = serverEvents.parseVector3(individualPlayerTickData[1]);
                            serverEvents.playerPastTargetRot = serverEvents.playerTargetRot;
                            serverEvents.playerTargetRot = Quaternion.Euler(serverEvents.parseVector3(individualPlayerTickData[2]));
                        }
                    }
                }
            }
            tick++;
            if(tick >= tickRate*replayTime){
                tick = 0;
            }
            serverEvents.resetSmoothTimer();
            if(enter){
                tick = startingTick;
            }
            yield return new WaitForSeconds(1f/((float)tickRate / replaySlowdown));
        }
        playerManager.deathMenu.SetActive(true);
        replayCam.gameObject.SetActive(false);
        cam.SetActive(true);
        serverEvents.replaying = false;
        playerManager.commitDie();
        fouxBullet.SetActive(false);
        //Debug.Log("ended replay");
    }

    private string MainListToString(List<List<string>> list) {
        string final = "[";
        foreach(List<string> listElement in list){
            final += "[";
            foreach(string tickListElement in listElement){
                final += tickListElement + ", ";
            }
            final += "]";
        }
        return final + "]";
    }
}
