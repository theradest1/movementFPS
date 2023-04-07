using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolReloadManager : MonoBehaviour
{
    public MapManager mapManager; 
    List<ToolRefill> refillSpots = new List<ToolRefill>();
    public GameObject refillPrefab;
    public WeaponManager weaponManager;

    public void spawnToolRefill(int spawnPosID, int type){
        spawnPosID %= mapManager.currentMap.toolSpawnPoints.Count;
        Vector3 pos = mapManager.currentMap.toolSpawnPoints[spawnPosID].transform.position;
        Debug.Log(spawnPosID + " - " + mapManager.currentMap.toolSpawnPoints.Count + " - " + refillSpots.Count);
        if(refillSpots[spawnPosID] != null){
            Destroy(refillSpots[spawnPosID].gameObject);
        }
        refillSpots[spawnPosID] = (Instantiate(refillPrefab, pos + Vector3.up / 2, Quaternion.Euler(0f, 0f, 90f)).GetComponent<ToolRefill>());
        refillSpots[spawnPosID].setInfo(weaponManager);
    }

    public void newMap(){
        if(refillSpots.Count > 0){
            Debug.Log("Count: " + refillSpots.Count);
            for (int i = 0; i < refillSpots.Count; i++){
                Destroy(refillSpots[i].gameObject);
            }
        }
        refillSpots = new List<ToolRefill>();
        for (int i = 0; i < mapManager.currentMap.toolSpawnPoints.Count; i++){
            refillSpots.Add(null);
        }
    }
}
