using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolReloadManager : MonoBehaviour
{
    public MapManager mapManager; 
    //List<Refill> refillSpots = new List<ToolRefill>();
    public GameObject refillPrefab;
    public WeaponManager weaponManager;

    public void spawnToolRefill(int spawnPosID, int type){
        /*spawnPosID %= mapManager.currentMap.toolSpawnPoints.Count;
        Vector3 pos = mapManager.currentMap.toolSpawnPoints[spawnPosID].transform.position;
        if(refillSpots[spawnPosID] != null){
            Destroy(refillSpots[spawnPosID].gameObject);
        }
        refillSpots[spawnPosID] = (Instantiate(refillPrefab, pos + Vector3.up / 2, Quaternion.Euler(0f, 0f, 90f)).GetComponent<ToolRefill>());
        refillSpots[spawnPosID].setInfo(weaponManager);*/
    }

    public void newMap(){
        /*if(refillSpots.Count > 0){
            for (int i = 0; i < refillSpots.Count; i++){
                if(refillSpots[i] != null){
                    Destroy(refillSpots[i].gameObject);
                }
            }
        }
        refillSpots = new List<ToolRefill>();
        for (int i = 0; i < mapManager.currentMap.toolSpawnPoints.Count; i++){
            refillSpots.Add(null);
        }*/
    }
}
