using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameGUIManager : MonoBehaviour
{
    GameObject menu;
    Slider senseSlider;
    Look look;
    GameObject killFeedObject;
    public GameObject killFeedChildPrefab;
    public float killFeedTime;

    // Start is called before the first frame update
    void Start()
    {
        killFeedObject = GameObject.Find("kill feed");
        menu = GameObject.Find("Menu");
        senseSlider = GameObject.Find("sense").GetComponent<Slider>();
        look = GameObject.Find("Main Camera").GetComponent<Look>();

        menu.SetActive(false);
    }

    public void killFeed(string killerUsername, string killedUsername){
        Debug.Log(killerUsername + " got " + killedUsername + " with a spatchula");
        GameObject newPart = Instantiate(killFeedChildPrefab, Vector3.zero, Quaternion.identity, killFeedObject.transform);
        newPart.GetComponent<TextMeshProUGUI>().text = killedUsername + " got " + killedUsername + " with a spatchula";
        Destroy(newPart, killFeedTime);
    }

    public void changeGUIState(){
        menu.SetActive(!menu.activeSelf);
        if(menu.activeSelf){
            Cursor.lockState = CursorLockMode.None;
            look.generalSense = 0f;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            look.generalSense = senseSlider.value;
        }
    }

    public void changeValue(){
        look.generalSense = senseSlider.value;
    }

    public void quit(){
        Application.Quit();
    }
}
