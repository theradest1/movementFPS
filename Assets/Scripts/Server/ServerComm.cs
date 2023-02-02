using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class ServerComm : MonoBehaviour
{
    UdpClient client;
    IPEndPoint remoteEndPoint;
    public GameObject player;
    //int throughPackets = 0;
    //int errorPackets = 0;
    public float updateSpeed;
    public ServerEvents serverEvents;
    int ID = -1;

    // Start is called before the first frame update
    void Start()
    {
        try{
            client = new UdpClient(5000);
            client.Connect("127.0.0.1", 4000);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, 4000);
        }
        catch(Exception e){
            Debug.LogError("Couldn't connect, exeption: " + e.Message);
        }
        ID = join("User" + UnityEngine.Random.Range(1000, 9999));
        Debug.Log("User ID: " + ID);
        InvokeRepeating("serverUpdate", .1f, updateSpeed);
        //InvokeRepeating("updatePPSGUI", 0f, 1f);
    }

    int join(string name)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes("newClient~" + name);
        client.Send(sendBytes, sendBytes.Length);
        
        //recieve
        byte[] receiveBytes = client.Receive(ref remoteEndPoint);
        return int.Parse(Encoding.ASCII.GetString(receiveBytes));
    }

    /*void updatePPSGUI(){
        Debug.Log("through packets: " + throughPackets);
        Debug.Log("error packets: " + errorPackets);

        throughPackets = 0;
        errorPackets = 0;
    }*/
    void Update() {
    }

    public void send(string message){
        byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        client.Send(sendBytes, sendBytes.Length);
    }

    void serverUpdate()
    {
        string info;
        try{
            //send
            byte[] sendBytes = Encoding.ASCII.GetBytes("update~" + ID + "~" + player.transform.position + "~" + player.transform.rotation);
            client.Send(sendBytes, sendBytes.Length);
            
            //recieve
            byte[] receiveBytes = client.Receive(ref remoteEndPoint);

            //throughPackets++;
            info = Encoding.ASCII.GetString(receiveBytes);
        }
        catch(Exception e){
            Debug.LogError(e.Message);
            //errorPackets++;
            return;
        }

        //Debug.Log(info);
        string[] rawEvents = info.Split('|');
        for(int i = 0; i < rawEvents.Length; i++){
            if(rawEvents[i] != ""){
                Debug.Log("___________________________________________");
                Debug.Log("Events recieved: " + rawEvents[i]);
                string[] splitRawEvents = rawEvents[i].Split("~");
                switch (splitRawEvents[0])
                {
                    case "update":
                        serverEvents.update(splitRawEvents[1], splitRawEvents[2], splitRawEvents[3]); //ID, position, rotation
                        break;
                    case "newClient":
                        serverEvents.newClient(splitRawEvents[1], splitRawEvents[2]); //ID, username
                        break;
                    default:
                        Debug.LogError("Event called that doesn't have a function: " + splitRawEvents[0]);
                        break;
                }
                
            }
        }
    }
}
