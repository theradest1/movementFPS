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
    public int usernameLengthLimit;
    public int usernameLengthMin;

    TMP_InputField portInput;
    TMP_InputField addressInput;
    TMP_InputField usernameInput;
    TMP_Dropdown IPDropdown;
    GameObject usernameWarning;
    Toggle inSchoolToggle;

    void Start(){
        usernameWarning = GameObject.Find("usernameWarning");
        usernameWarning.SetActive(false);
        IPDropdown = GameObject.Find("IPs").GetComponent<TMP_Dropdown>();
        portInput = GameObject.Find("port input").GetComponent<TMP_InputField>();
        addressInput = GameObject.Find("address input").GetComponent<TMP_InputField>();
        usernameInput = GameObject.Find("username input").GetComponent<TMP_InputField>();

        updateInfo();
    }

    // Update is called once per frame
    public void updateInfo(){
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
        if(usernameInput.text.Length < usernameLengthMin){
            usernameWarning.SetActive(true);
        }
        else{
            usernameWarning.SetActive(false);
        }

        username = usernameInput.text;

        string option = IPDropdown.options[IPDropdown.value].text;
        
        if(option == "School"){
            port = 4000;
            address = "10.100.4.86";
        }
        else if(option == "General"){
            port = 4000;
            address = "75.100.205.73";
        }
        else if(option == "Local Host"){
            port = 4000;
            address = "localhost";
        }
        else if(option == "Custom"){
            port = int.Parse(portInput.text);
            address = addressInput.text;
        }
    }

    public void join(){
        if(usernameInput.text.Length >= usernameLengthMin){
            SceneManager.LoadScene(1);
        }
    }

    public void quit(){
        Application.Quit();
    }
}
