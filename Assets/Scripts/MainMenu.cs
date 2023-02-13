using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static int port = 4000;
    //public static int clientPort = 5000;
    public static string address = "localhost";//"192.168.0.50";
    public static string username = "joe";
    public static bool inSchool = true;
    public int usernameLengthLimit;

    TMP_InputField portInput;
    TMP_InputField addressInput;
    TMP_InputField usernameInput;
    Toggle inSchoolToggle;

    void Start(){
        portInput = GameObject.Find("port input").GetComponent<TMP_InputField>();
        addressInput = GameObject.Find("address input").GetComponent<TMP_InputField>();
        usernameInput = GameObject.Find("username input").GetComponent<TMP_InputField>();
        inSchoolToggle = GameObject.Find("schoolwifi").GetComponent<Toggle>();

        updateInfo();
    }

    // Update is called once per frame
    public void updateInfo(){
        inSchool = inSchoolToggle.isOn;
        string allowedChars = "1234567890abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUFWYXZ_";
		int strLen = usernameInput.text.Length;
		if(strLen >= 1){
			if(!allowedChars.Contains(usernameInput.text[strLen - 1])){
				usernameInput.text = usernameInput.text.Substring(0, strLen - 1);
			}
		}
		if(usernameInput.text.Length > usernameLengthLimit){
			usernameInput.text = usernameInput.text.Substring(0, usernameLengthLimit);
		}
        port = int.Parse(portInput.text);
        //clientPort = int.Parse(clientPortInput.text);
        address = addressInput.text;
        username = usernameInput.text;
    }

    public void join(){
        SceneManager.LoadScene(1);
    }

    public void quit(){
        Application.Quit();
    }
}
