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
    public Toggle debugToggle;
    public PostProcessLayer postLayer;
    public PostProcessLayer postLayerScope;
    public GameObject debugContainer;
    public Weapons weapons;

    public GameObject generalMenu;
    public GameObject graphicsMenu;
    public GameObject controlsMenu;

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


        //Dont clean this chunk up, it is so things don't get overwriten (you already tried to clean it up)
        float sense = PlayerPrefs.GetFloat("Sensitivity", 1f);
        float senseADS = PlayerPrefs.GetFloat("SensitivityADS", 1f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        bool vsync = PlayerPrefs.GetInt("Vsync", 1) == 1;
        bool ads = PlayerPrefs.GetInt("ToggleADS", 1) == 1;
        bool post = PlayerPrefs.GetInt("Post", 1) == 1;
        bool debug = PlayerPrefs.GetInt("Debug", 1) == 1;
        int quality = PlayerPrefs.GetInt("Quality", 5);

        senseSlider.value = sense;
        senseSliderADS.value = senseADS;
        volumeSlider.value = volume;
        fullscreenToggle.isOn = fullscreen;
        vsyncToggle.isOn = vsync;
        toggleADS.isOn = ads;
        postToggle.isOn = post;
        qualitySlider.value = quality;
        debugToggle.isOn = debug;

        menu.SetActive(false);
        changeValue();
        InvokeRepeating("updateGameClock", 0f, 1f);
    }

    public void openMenuSection(GameObject menuToOpen){
        generalMenu.SetActive(false);
        controlsMenu.SetActive(false);
        graphicsMenu.SetActive(false);

        menuToOpen.SetActive(true);
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
            generalMenu.SetActive(true);
            controlsMenu.SetActive(false);
            graphicsMenu.SetActive(false);
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
        if(fullscreenToggle.isOn){
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        else{
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
        weapons.toggleScoping = toggleADS.isOn;

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
        debugContainer.SetActive(debugToggle.isOn);

        QualitySettings.SetQualityLevel((int)qualitySlider.value, true);
        PlayerPrefs.SetFloat("Sensitivity", senseSlider.value);
        PlayerPrefs.SetFloat("SensitivityADS", senseSliderADS.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.SetInt("Quality", (int)qualitySlider.value);
        PlayerPrefs.SetInt("Vsync", QualitySettings.vSyncCount);
        
        if(debugToggle.isOn){
            PlayerPrefs.SetInt("Debug", 1);
        }
        else{
            PlayerPrefs.SetInt("Debug", 0);
        }
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
        scoreboard.SetActive(controlsManager.openScoreBoard);
    }
}
