using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

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
    public Slider qualitySlider;
    public GameObject killFeedObject;
    public GameObject killFeedChildPrefab;
    public GameObject scoreboard;
    public TextMeshProUGUI gameClock;
    public TMP_InputField senseInputText;
    public TMP_InputField senseInputTextADS;
    public Toggle fullscreenToggle;
    public Toggle toggleADS;
    public Toggle postToggle;
    public Toggle vsyncToggle;
    public PostProcessLayer postLayer;
    public PostProcessLayer postLayerScope;

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

        senseSlider.value = PlayerPrefs.GetFloat("Sensitivity", 1f);
        senseSliderADS.value = PlayerPrefs.GetFloat("SensitivityADS", 1f);
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        vsyncToggle.isOn = PlayerPrefs.GetInt("Vsync", 1) == 1;
        toggleADS.isOn = PlayerPrefs.GetInt("ToggleADS", 1) == 1;
        postToggle.isOn = PlayerPrefs.GetInt("Post", 1) == 1;
        qualitySlider.value = PlayerPrefs.GetInt("Quality", 5);


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
        Debug.Log("Updated settings");
        look.generalSense = senseSlider.value;
        look.generalSenseADS = senseSliderADS.value;
        senseInputText.text = (senseSlider.value * 100)  + "";
        senseInputTextADS.text = (senseSliderADS.value * 100)  + "";
        soundManager.generalVolume = volumeSlider.value;
        if(fullscreenToggle.isOn){
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else{
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        if(toggleADS.isOn){
            controlsManager.toggleADS = true;
        }
        else{
            controlsManager.toggleADS = false;
        }
        if(postToggle.isOn){
            postLayer.enabled = true;
            postLayerScope.enabled = true;
        }
        else{
            postLayer.enabled = false;
            postLayerScope.enabled = false;
        }
        if(postToggle.isOn){
            QualitySettings.vSyncCount = 1;
        }
        else{
            QualitySettings.vSyncCount = 0;
        }
        QualitySettings.SetQualityLevel((int)qualitySlider.value, true);
        PlayerPrefs.SetFloat("Sensitivity", senseSlider.value);
        PlayerPrefs.SetFloat("SensitivityADS", senseSliderADS.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Quality", (int)qualitySlider.value);
        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);
        if(fullscreenToggle.isOn){
            PlayerPrefs.SetInt("Fullscreen", 1);
        }
        else{
            PlayerPrefs.SetInt("Fullscreen", 0);
        }
        if(toggleADS.isOn){
            PlayerPrefs.SetInt("ToggleADS", 1);
        }
        else{
            PlayerPrefs.SetInt("ToggleADS", 0);
        }
        if(postToggle.isOn){
            PlayerPrefs.SetInt("Post", 1);
        }
        else{
            PlayerPrefs.SetInt("Post", 0);
        }
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
