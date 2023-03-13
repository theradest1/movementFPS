using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    //references
    //TMP_InputField portInput;
    //TMP_InputField addressInput;
    TMP_InputField usernameInput;
    //TMP_Dropdown IPDropdown;
    GameObject usernameWarning;
    //TextMeshProUGUI serverStatus;

    //variables
    UdpClient client;
    IPEndPoint remoteEndPoint;
    [Header("References:")]
    public List<ServerInfo> servers;

    [Header("Server settings:")]
    public int usernameLengthLimit;
    //public int delayBeforeOffline = 3000;
    public int usernameLengthMin;

    [HideInInspector]
    public static int port = 4000;
    [HideInInspector]
    public static string address = "localhost";//"192.168.0.50";
    [HideInInspector]
    public static string username = "joe";


    void Start(){
        //serverStatus = GameObject.Find("server status").GetComponent<TextMeshProUGUI>();
        usernameWarning = GameObject.Find("usernameWarning");
        usernameWarning.SetActive(false);
        //IPDropdown = GameObject.Find("IPs").GetComponent<TMP_Dropdown>();
        //portInput = GameObject.Find("port input").GetComponent<TMP_InputField>();
        //addressInput = GameObject.Find("address input").GetComponent<TMP_InputField>();
        usernameInput = GameObject.Find("username input").GetComponent<TMP_InputField>();

        usernameInput.text = PlayerPrefs.GetString("Username", "");
        //IPDropdown.value = PlayerPrefs.GetInt("Server", 0);

        updateInfo();
        refreshServerList();
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
    }

    public void joinServer(string info){
        if(usernameInput.text.Length >= usernameLengthMin){
            PlayerPrefs.SetString("Username", usernameInput.text);
            //PlayerPrefs.SetInt("Server", IPDropdown.value);
            PlayerPrefs.Save();

            string[] splitInfo = info.Split("~");
            address = splitInfo[0];
            port = int.Parse(splitInfo[1]);
        
            SceneManager.LoadScene(1);
        }
    }

    public void refreshServerList(){
        for(int i = 0; i < servers.Count; i++){
            servers[i].checkStatus();
        }
    }

    public void quit(){
        Application.Quit();
    }
}
