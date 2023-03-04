using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class InGameGUIManager : MonoBehaviour
{
    public GameObject menu;
    public Slider senseSlider;
    public Slider volumeSlider;
    Look look;
    SoundManager soundManager;
    ControlsManager controlsManager;
    ServerEvents serverEvents;
    public GameObject killFeedObject;
    public GameObject killFeedChildPrefab;
    public float killFeedTime;
    public GameObject scoreboard;
    public TMP_InputField senseInputText;

    Image hitMarker;
    public float hitMarkerChangeSpeed;

    

    // Start is called before the first frame update
    void Start()
    {
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        hitMarker = GameObject.Find("hit marker").GetComponent<Image>();
        look = GameObject.Find("Main Camera").GetComponent<Look>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();

        float sense = PlayerPrefs.GetFloat("Sensitivity", 1f);
        float volume = PlayerPrefs.GetFloat("Volume", 1f);

        senseSlider.value = sense;
        volumeSlider.value = volume;

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

    public void setSenseWithText(){
        try
        {
            senseSlider.value = float.Parse(senseInputText.text)/100;
            changeValue();
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public void changeValue(){
        look.generalSense = senseSlider.value;
        senseInputText.text = (senseSlider.value * 100)  + "";
        soundManager.generalVolume = volumeSlider.value;
        PlayerPrefs.SetFloat("Sensitivity", senseSlider.value);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    public void quit(){
        serverEvents.leave();
        SceneManager.LoadScene(0);
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
