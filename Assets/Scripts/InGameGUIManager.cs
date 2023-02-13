using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameGUIManager : MonoBehaviour
{
    GameObject menu;
    Slider senseSlider;
    look lookScript;

    // Start is called before the first frame update
    void Start()
    {
        menu = GameObject.Find("Menu");
        senseSlider = GameObject.Find("sense").GetComponent<Slider>();
        lookScript = GameObject.Find("Main Camera").GetComponent<look>();

        menu.SetActive(false);
    }

    public void changeGUIState(){
        menu.SetActive(!menu.activeSelf);
        if(menu.activeSelf){
            Cursor.lockState = CursorLockMode.None;
            lookScript.generalSense = 0f;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            lookScript.generalSense = senseSlider.value;
        }
    }

    public void changeValue(){
        lookScript.generalSense = senseSlider.value;
    }

    public void quit(){
        Application.Quit();
    }
}
