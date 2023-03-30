using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameGUIManager : MonoBehaviour
{
    Look look;
    SoundManager soundManager;
    ControlsManager controlsManager;
    ServerEvents serverEvents;
    Image hitMarker;

    [Header("References:")]
    public GameObject menu;
    public Slider senseSlider;
    public Slider senseSliderADS;
    public Slider volumeSlider;
    public GameObject killFeedObject;
    public GameObject killFeedChildPrefab;
    public GameObject scoreboard;
    public TextMeshProUGUI gameClock;
    public TMP_InputField senseInputText;
    public TMP_InputField senseInputTextADS;

    [Header("Settings:")]
    public float killFeedTime;
    public float hitMarkerChangeSpeed;

    [HideInInspector]
    public float secondsUntilMapChange = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        hitMarker = GameObject.Find("hit marker").GetComponent<Image>();
        look = GameObject.Find("Main Camera").GetComponent<Look>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();

        float sense = PlayerPrefs.GetFloat("Sensitivity", 1f);
        float senseADS = PlayerPrefs.GetFloat("SensitivityADS", 1f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        senseSlider.value = sense;
        senseSliderADS.value = senseADS;
        volumeSlider.value = volume;

        menu.SetActive(false);
        changeValue();
        InvokeRepeating("updateGameClock", 0f, 1f);
    }

    void updateGameClock(){
        secondsUntilMapChange--;
        if(secondsUntilMapChange > 0){
            int min = (int) Mathf.Floor(secondsUntilMapChange/60);
            int sec = (int) Mathf.Round(secondsUntilMapChange%60);
            string clock = "";
            if(min < 10){
                clock += "0";
            }
            clock += min + " : ";
            if(sec < 10){
                clock += "0";
            }
            clock += sec;

            gameClock.text = clock;
        }
        else{
            gameClock.text = "00 : 00";
        }
    }

    public void killFeed(string killerUsername, string killedUsername){
        GameObject newPart = Instantiate(killFeedChildPrefab, Vector3.zero, Quaternion.identity, killFeedObject.transform);
        newPart.GetComponent<TextMeshProUGUI>().text = killerUsername + " got " + killedUsername + " with a spatchula";
        Destroy(newPart, killFeedTime);
    }

    public void changeGUIState(){
        menu.SetActive(!menu.activeSelf);
        if(menu.activeSelf){
            look.generalSense = 0f;
            controlsManager.inMenu = true;
        }
        else{
            controlsManager.inMenu = false;
            look.generalSense = senseSlider.value;
        }
    }

    public void setSenseWithText(){
        try
        {
            senseSlider.value = float.Parse(senseInputText.text)/100;
            senseSliderADS.value = float.Parse(senseInputTextADS.text)/100;
            changeValue();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void changeValue(){
        look.generalSense = senseSlider.value;
        look.generalSenseADS = senseSliderADS.value;
        senseInputText.text = (senseSlider.value * 100)  + "";
        senseInputTextADS.text = (senseSliderADS.value * 100)  + "";
        soundManager.generalVolume = volumeSlider.value;
        PlayerPrefs.SetFloat("Sensitivity", senseSlider.value);
        PlayerPrefs.SetFloat("SensitivityADS", senseSliderADS.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void quit(){
        serverEvents.leave();
        SceneManager.LoadScene(0);
    }
    public void retry(){
        serverEvents.leave();
        SceneManager.LoadScene(2);
    }

    public void hit(bool crit){
        if(crit){
            hitMarker.color = new Color(1, 0, 0, 1);
        }
        else{
            hitMarker.color = new Color(1, 1, 1, 1);
        }
    }

    private void Update() {
        hitMarker.color = new Color(1, hitMarker.color.g + hitMarkerChangeSpeed * Time.deltaTime, hitMarker.color.b + hitMarkerChangeSpeed * Time.deltaTime, hitMarker.color.a - hitMarkerChangeSpeed * Time.deltaTime);
        scoreboard.SetActive(controlsManager.tab);
    }
}
