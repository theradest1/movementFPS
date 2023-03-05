using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariableUpdater : MonoBehaviour
{

    public GameObject weapons;
    public GameObject classes;

    public void updateVars(string allData){
        //Debug.Log(allData);
        string[] varChunks = allData.Split("~");
        for(int i = 0; i < varChunks.Length; i++){
            string[] vars = varChunks[i].Split(";");

            if(classes.transform.Find(vars[0]) != null){
                classes.transform.Find(vars[0]).GetComponent<ClassInfo>().setVars(vars);
            }
            else if(weapons.transform.Find(vars[0]) != null){
                weapons.transform.Find(vars[0]).GetComponent<WeaponInfo>().setVars(vars);
            }
        }
    }
}
