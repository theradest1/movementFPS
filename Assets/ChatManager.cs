using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ChatManager : MonoBehaviour
{
    float timer;
    bool ableToOpenChat = true;
    bool currentlyOpen;
    int totalMesssages = 0;
    
    [Header("References:")]
    public GameObject chatChildPrefab;
    public GameObject chatParentObject;
    public GameObject messageContainer;
    public ControlsManager controlsManager;
    public ServerEvents serverEvents;
    public ServerComm serverComm;
    public TMP_InputField inputText;

    [Header("Settings:")]
    public float timeBeforeHide;
    public int messagesUntillDelete = 5;

    private void Start() {
        inputText.enabled = true;
    }

    private void Update() {
        timer -= Time.deltaTime;

        if(controlsManager.chat && ableToOpenChat){
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
            timer = timeBeforeHide;
        }
        else{
            controlsManager.chatting = false;
        }
        
        if(!controlsManager.chat){
            ableToOpenChat = true;
        }

        if(timer <= 0){
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
        }
        inputText.enabled = false;
        ableToOpenChat = false;
    }

    public void newChat(string message, Color messageColor, int chatterID = -1){
        totalMesssages++;
        if(totalMesssages >= messagesUntillDelete){
            totalMesssages--;
            Destroy(messageContainer.transform.GetChild(0).gameObject);
        }

        //Debug.Log("Player with ID " + chatterID + " said \"" + message + "\"");
        timer = timeBeforeHide;
        if(chatterID != -1){
            if(chatterID == serverComm.ID){
                message = "You: " + message;
            }
            else{
                message = GameObject.Find(chatterID + "").GetComponent<OtherPlayer>().username + ": " + message;
            }
        }
        TextMeshProUGUI newMessageObject = Instantiate(chatChildPrefab, messageContainer.transform).GetComponent<TextMeshProUGUI>();
        newMessageObject.text = message;
        newMessageObject.color = messageColor;
        Invoke("UpdateParentLayoutGroup", 0.1f);
    }

    void UpdateParentLayoutGroup(){
        chatParentObject.SetActive(false);
        chatParentObject.SetActive(true);
    }
}
