using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class OtherPlayer : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public Canvas usernameCanvas;
    GameObject playerCam;
    public float health = 100f;

    public void setUsername(string usrname){
        usernameText.text = usrname;
    }

    void Start(){
        playerCam = GameObject.Find("Main Camera");
        Debug.Log(playerCam);
    }

    void Update(){
        usernameCanvas.gameObject.transform.LookAt(playerCam.transform);
    }
}
