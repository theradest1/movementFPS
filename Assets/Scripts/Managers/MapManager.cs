using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapInfo> maps;
    MapInfo currentMap;
    PlayerManager playerManager;
    ServerComm serverComm;
    //ServerEvents serverEvents;

    private void Start() {
        //serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        currentMap = playerManager.currentMap;
    }

    public void newMap(int newMapID){
        currentMap.gameObject.SetActive(false);
        currentMap = maps[newMapID];
        currentMap.gameObject.SetActive(true);
        playerManager.currentMap = currentMap;
        playerManager.commitDie();
        Debug.Log("howdy");
    }

    public void skipMap(){
        serverComm.send("skipMap~" + serverComm.ID);
    }
}
