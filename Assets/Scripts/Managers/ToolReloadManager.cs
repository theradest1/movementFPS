using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolReloadManager : MonoBehaviour
{
    public MapManager mapManager; 
    public GameObject refillPrefab;
    public WeaponManager weaponManager;
    public void spawnToolRefill(int spawnPosID, int type){
        Vector3 pos = mapManager.currentMap.toolSpawnPoints[spawnPosID%(mapManager.currentMap.toolSpawnPoints.Count)].transform.position;
        Instantiate(refillPrefab, pos + Vector3.up / 2, Quaternion.Euler(0f, 0f, 90f)).GetComponent<ToolRefill>().setInfo(weaponManager);
    }
}
