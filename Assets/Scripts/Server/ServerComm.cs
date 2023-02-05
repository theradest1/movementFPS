using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class ServerComm : MonoBehaviour
{
    UdpClient client;
    IPEndPoint remoteEndPoint;
    public GameObject player;
    int throughPackets = 0;
    //int errorPackets = 0;
    public float updateSpeed;
    public ServerEvents serverEvents;
    public int ID = -1;
    public int SERVERPORT;
    public int CLIENTPORT;
    public string SERVERADDRESS;
    public TextMeshProUGUI PPSText;

    // Start is called before the first frame update
    void Start()
    {
        SERVERADDRESS = MainMenu.address;
        CLIENTPORT = MainMenu.clientPort;
        SERVERPORT = MainMenu.port;

        PPSText.text = "PPS: lots";
        try{
            client = new UdpClient(/*CLIENTPORT*/);
            client.Connect(SERVERADDRESS, SERVERPORT);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, SERVERPORT);
        }
        catch(Exception e){
            Debug.LogError("Couldn't connect, exeption: " + e.Message);
        }
        ID = join("User" + MainMenu.username);
        Debug.Log("User ID: " + ID);
        InvokeRepeating("runServerUpdate", .1f, updateSpeed);
        InvokeRepeating("updatePPSGUI", 0f, 1f);
    }

    int join(string name)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes("newClient~" + name);
        client.Send(sendBytes, sendBytes.Length);
        
        //recieve
        byte[] receiveBytes = client.Receive(ref remoteEndPoint);
        return int.Parse(Encoding.ASCII.GetString(receiveBytes));
    }

    void updatePPSGUI(){
        //Debug.Log("through packets: " + throughPackets);
        //Debug.Log("error packets: " + errorPackets);

        PPSText.text = "PPS: " + throughPackets;
        throughPackets = 0;
    }

    void Update() {
    }

    public void send(string message){
        byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        client.Send(sendBytes, sendBytes.Length);
        throughPackets++;
    }

    void runServerUpdate(){
        serverUpdate();
    }

    async void serverUpdate()
    {
        string info = "";
        byte[] sendBytes = Encoding.ASCII.GetBytes("u~" + ID + "~" + player.transform.position + "~" + player.transform.rotation);
        client.Send(sendBytes, sendBytes.Length);
        throughPackets++;


        //recieve
        byte[] receiveBytes = new byte[0];
        await Task.Run(() => receiveBytes = client.Receive(ref remoteEndPoint));
        info = Encoding.ASCII.GetString(receiveBytes);
        serverEvents.resetSmoothTimer();
        
        //Debug.Log("___________________________________________");
        //Debug.Log("Events recieved: " + info);
        string[] rawEvents = info.Split('|');
        for(int i = 0; i < rawEvents.Length; i++){
            if(rawEvents[i] != ""){
                string[] splitRawEvents = rawEvents[i].Split("~");
                switch (splitRawEvents[0])
                {
                    case "u":
                        serverEvents.update(splitRawEvents[1], splitRawEvents[2], splitRawEvents[3]); //ID, position, rotation
                        break;
                    case "newClient":
                        serverEvents.newClient(splitRawEvents[1], splitRawEvents[2]); //ID, username
                        break;
                    case "removeClient":
                        serverEvents.removeClient(splitRawEvents[1]); //ID
                        break;
                    default:
                        Debug.LogError("Event called that doesn't have a function: " + splitRawEvents[0]);
                        break;
                }
                
            }
        }
        //yield return null; 

    }

    //byte[] waitForResponse(ref remoteEndPoint){

    //}
}
