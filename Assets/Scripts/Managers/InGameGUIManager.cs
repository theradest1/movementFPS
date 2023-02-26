using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameGUIManager : MonoBehaviour
{
    public GameObject menu;
    public Slider senseSlider;
    public Slider volumeSlider;
    Look look;
    SoundManager soundManager;
    ControlsManager controlsManager;
    public GameObject killFeedObject;
    public GameObject killFeedChildPrefab;
    public float killFeedTime;
    public GameObject scoreboard;

    

    // Start is called before the first frame update
    void Start()
    {
        //killFeedObject = GameObject.Find("kill feed");
        //menu = GameObject.Find("Menu");
        //senseSlider = GameObject.Find("sense").GetComponent<Slider>();
        //volumeSlider = GameObject.Find("volume").GetComponent<Slider>();
        look = GameObject.Find("Main Camera").GetComponent<Look>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();

        menu.SetActive(false);
        changeValue();
    }

    public void killFeed(string killerUsername, string killedUsername){
        GameObject newPart = Instantiate(killFeedChildPrefab, Vector3.zero, Quaternion.identity, killFeedObject.transform);
        newPart.GetComponent<TextMeshProUGUI>().text = killerUsername + " got " + killedUsername + " with a spatchula";
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
        soundManager.generalVolume = volumeSlider.value;
    }

    public void quit(){
        Application.Quit();
    }

    private void Update() {
        scoreboard.SetActive(controlsManager.tab);
    }
}
