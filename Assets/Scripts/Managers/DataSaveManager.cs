using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataSaveManager : MonoBehaviour
{
    public TextAsset saveFile;

    public string getAllData(){
        return saveFile.text;
    }

    /*public string getDataByName(string name){
        string allData = getAllData();
    }*/
}
