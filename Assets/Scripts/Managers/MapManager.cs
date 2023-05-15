using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapManager : MonoBehaviour
{
    public List<MapInfo> maps;
    public List<TextMeshProUGUI> lables;
    [HideInInspector]
    public MapInfo currentMap;
    PlayerManager playerManager;
    ServerComm serverComm;
    ServerEvents serverEvents;
    ToolReloadManager toolReloadManager;
    public GameObject mapChoosingMenu;
    ControlsManager controlsManager;
    int currentMapVote = -1;
    //ServerEvents serverEvents;
    Light sun;

    private void Start() {
        toolReloadManager = GameObject.Find("manager").GetComponent<ToolReloadManager>();
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        currentMap = playerManager.currentMap;
        toolReloadManager.newMap();
        sun = GameObject.Find("sun").GetComponent<Light>();
    }

    public void newMap(int newMapID){
        currentMap.gameObject.SetActive(false);
        currentMap = maps[newMapID];
        currentMap.gameObject.SetActive(true);
        playerManager.currentMap = currentMap;
        mapChoosingMenu.SetActive(false);
        controlsManager.choosingMap = false;
        playerManager.commitDie();
        toolReloadManager.newMap();

        sun.color = currentMap.sunColor;
        sun.intensity = currentMap.sunIntensity;

        RenderSettings.skybox = currentMap.skyboxMaterial;
    }

    public void skipMap(){
        serverComm.send("skipMap~" + serverComm.ID);
    }

    public void voteForMap(int mapID){
        serverComm.send("voteMap~" + serverComm.ID + "~" + mapID);
        serverEvents.sendEvent("ue", "voteMap", mapID + "");
    }

    public void chooseMap(){
        controlsManager.choosingMap = true;
        mapChoosingMenu.SetActive(true);
        playerManager.commitDie();
    }

    public void setVote(int ID, int mapID){
        if(ID != serverComm.ID){
            OtherPlayer playerScript = serverEvents.clientScripts[serverEvents.clientIDs.IndexOf(ID)];
            if(playerScript.currentMapVote == -1){
                lables[mapID].text = int.Parse(lables[mapID].text) + 1 + "";
                playerScript.currentMapVote = mapID;
            }
            else{
                lables[playerScript.currentMapVote].text = int.Parse(lables[playerScript.currentMapVote].text) - 1 + "";
                playerScript.currentMapVote = mapID;
                lables[playerScript.currentMapVote].text = int.Parse(lables[playerScript.currentMapVote].text) + 1 + "";
            }
        }
        else{
            if(currentMapVote == -1){
                lables[mapID].text = int.Parse(lables[mapID].text) + 1 + "";
                currentMapVote = mapID;
            }
            else{
                lables[currentMapVote].text = int.Parse(lables[currentMapVote].text) - 1 + "";
                currentMapVote = mapID;
                lables[currentMapVote].text = int.Parse(lables[currentMapVote].text) + 1 + "";
            }
        }
    }
}
