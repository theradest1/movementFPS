using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviour
{
    public GameObject chatChildPrefab;
    List<GameObject> chatChildObjects;
    public GameObject chatParentObject;
    public float timeBeforeHide;
    public float timer;
    public bool ableToOpenChat = true;
    bool currentlyOpen;
    public ControlsManager controlsManager;
    public ServerEvents serverEvents;
    public TMP_InputField  inputText;

    private void Start() {
        inputText.enabled = true;
    }

    private void Update() {
        timer -= Time.deltaTime;

        if(controlsManager.enter && ableToOpenChat){
            if(!inputText.isFocused){
                timer = timeBeforeHide;
                inputText.enabled = true;
                chatParentObject.SetActive(true);
                inputText.ActivateInputField();
                ableToOpenChat = false;
            }
        }

        if(inputText.isFocused){
            controlsManager.chatting = true;
        }
        else{
            controlsManager.chatting = false;
        }
        
        if(!controlsManager.enter){
            ableToOpenChat = true;
        }




        if(!inputText.isFocused && timer <= 0){
            chatParentObject.SetActive(false);
        }
        else{
            chatParentObject.SetActive(true);
        }
    }

    public void sendMessage(){
        timer = timeBeforeHide;
        if(inputText.text != ""){
            serverEvents.sendEvent("ue", "chat", inputText.text);
            inputText.text = "";
            Debug.Log("disabled");
            inputText.enabled = false;
            ableToOpenChat = false;
        }
    }

    public void newChat(int chatterID, string message){
        Debug.Log("Player with ID " + chatterID + " said \"" + message + "\"");
        timer = timeBeforeHide;
    }
}
