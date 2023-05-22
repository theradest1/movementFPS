using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableUpdater : MonoBehaviour
{

    public GameObject weapons;
    public Abilities abilities;
    public Tools tools;
    public movement movementScript;
    public PlayerManager playerManager;
    public GameObject throwableInfos;

    public void updateVars(string allData){
        string[] varChunks = allData.Split("~");
        for(int i = 0; i < varChunks.Length; i++){

            string[] vars = varChunks[i].Split(";");
            Debug.Log(vars[0]);
            if(weapons.transform.Find(vars[0]) != null){
                weapons.transform.Find(vars[0]).GetComponent<WeaponInfo>().setVars(vars);
            }
            else if(throwableInfos.transform.Find(vars[0])){
                throwableInfos.transform.Find(vars[0]).GetComponent<ThrowableInfo>().setVars(vars);
            }
            else if(vars[0] == "Abilities"){
                abilities.setVars(vars);
            }
            else if(vars[0] == "Tools"){
                tools.setVars(vars);
            }
            else if(vars[0] == "Player"){
                playerManager.setVars(vars);
            }
            else if(vars[0] == "Movement"){
                movementScript.setVars(vars);
            }
        }
    }
}
