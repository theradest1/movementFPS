using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class ServerInfo : MonoBehaviour
{
    UdpClient client;
    IPEndPoint remoteEndPoint;

    public string serverName;
    public string IP;
    public int port;
    public GameObject connectionIndicator;
    public TextMeshProUGUI playerCount;
    public int timoutTime = 1000;

    [Header("Only for custom server")]
    public TMP_InputField customAddressInput;
    public TMP_InputField customPortInput;

    async public void checkStatus(){
        try{
            client = new UdpClient(/*CLIENTPORT*/);
            if(serverName == "custom"){
                port = int.Parse(customPortInput.text);
                IP = customAddressInput.text;
            }
            client.Connect(IP, port);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, port);

            byte[] sendBytes = Encoding.ASCII.GetBytes("youOnBruv");
            client.Send(sendBytes, sendBytes.Length);

            byte[] receiveBytes = Encoding.ASCII.GetBytes("EMPTY");
            await Task.WhenAny(Task.Run(() => receiveBytes = client.Receive(ref remoteEndPoint)), Task.Delay(timoutTime));

            string info = Encoding.ASCII.GetString(receiveBytes);
            if(info == "EMPTY"){
                connectionIndicator.SetActive(true);
            }
            else{
                connectionIndicator.SetActive(false);
                if(serverName != "custom"){
                    playerCount.text = info;
                }
            }
        }
        catch(Exception e){
            Debug.LogError("Couldn't connect, exeption: " + e.Message);
        }
    }
}
