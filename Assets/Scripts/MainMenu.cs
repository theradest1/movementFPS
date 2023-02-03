using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static int port;
    public static int clientPort;
    public static string address;
    public static string username;

    public TMP_InputField portInput;
    public TMP_InputField clientPortInput;
    public TMP_InputField addressInput;
    public TMP_InputField usernameInput;

    void Start(){
        updateInfo();
        Debug.Log("howdy");
    }

    // Update is called once per frame
    public void updateInfo(){
        port = int.Parse(portInput.text);
        clientPort = int.Parse(clientPortInput.text);
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
