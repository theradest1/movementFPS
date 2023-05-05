using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableUpdater : MonoBehaviour
{

    public GameObject weapons;

    public void updateVars(string allData){
        string[] varChunks = allData.Split("~");
        for(int i = 0; i < varChunks.Length; i++){

            string[] vars = varChunks[i].Split(";");
            Debug.Log(vars[0]);
            if(weapons.transform.Find(vars[0]) != null){
                weapons.transform.Find(vars[0]).GetComponent<WeaponInfo>().setVars(vars);
            }
        }
    }
}
