using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int port = 4000;
    //public static int clientPort = 5000;
    public static string address = "localhost";//"192.168.0.50";
    public static string username = "joe";
    public int usernameLengthLimit;

    public TMP_InputField portInput;
    //public TMP_InputField clientPortInput;
    public TMP_InputField addressInput;
    public TMP_InputField usernameInput;

    void Start(){
        updateInfo();
        Debug.Log("howdy");
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
