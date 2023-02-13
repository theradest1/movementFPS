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
    int SERVERPORT;
    string SERVERADDRESS;

    ServerEvents serverEvents;
    GameObject player;
    TextMeshProUGUI PPSText;
    TextMeshProUGUI BPSText;
    TextMeshProUGUI latencyText;

    int throughPackets = 0;
    int bytes = 0;
    bool inSchool;

    public float updateSpeed;
    public int ID = -1;
    //public int CLIENTPORT;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        PPSText = GameObject.Find("PPS debug").GetComponent<TextMeshProUGUI>();
        BPSText = GameObject.Find("BPS debug").GetComponent<TextMeshProUGUI>();
        latencyText = GameObject.Find("Latency Debug").GetComponent<TextMeshProUGUI>();

        SERVERADDRESS = MainMenu.address;
        //CLIENTPORT = MainMenu.clientPort;
        SERVERPORT = MainMenu.port;
        inSchool = MainMenu.inSchool;

        PPSText.text = "PPS: lots";
        try{
            client = new UdpClient(/*CLIENTPORT*/);
            client.Connect(SERVERADDRESS, SERVERPORT);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, SERVERPORT);
        }
        catch(Exception e){
            Debug.LogError("Couldn't connect, exeption: " + e.Message);
        }
        ID = join(MainMenu.username);
        Debug.Log("User ID: " + ID);
        InvokeRepeating("serverUpdate", .1f, updateSpeed);
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
        BPSText.text = "BPS: " + bytes;
        bytes = 0;
        PPSText.text = "PPS: " + throughPackets;
        throughPackets = 0;
    }

    public void send(string message){
        byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        bytes += sendBytes.Length;
        client.Send(sendBytes, sendBytes.Length);
        throughPackets++;
    }

    async void serverUpdate()
    {
        if(inSchool){
            try{
                client = new UdpClient(/*CLIENTPORT*/);
                client.Connect(SERVERADDRESS, SERVERPORT);
                remoteEndPoint = new IPEndPoint(IPAddress.Any, SERVERPORT);
                //Debug.Log("howdy more");
            }
            catch(Exception e){
                Debug.LogError("Couldn't connect, exeption: " + e.Message);
            }
        }

        string info = "";
        byte[] sendBytes = Encoding.ASCII.GetBytes("u~" + ID + "~" + player.transform.position + "~" + player.transform.rotation);
        bytes += sendBytes.Length;
        client.Send(sendBytes, sendBytes.Length);
        throughPackets++;


        //recieve
        byte[] receiveBytes = new byte[0];
        
        float latencyTimer = Time.time;
        await Task.Run(() => receiveBytes = client.Receive(ref remoteEndPoint));
        latencyText.text = "Latency: " + Mathf.Round((Time.time - latencyTimer) * 1000f);

        info = Encoding.ASCII.GetString(receiveBytes);
        serverEvents.resetSmoothTimer();
        
        //Debug.Log("___________________________________________");
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
                    case "damage":
                        //Debug.Log(rawEvents[i]);
                        serverEvents.damage(splitRawEvents[1], splitRawEvents[2], splitRawEvents[3]); //attacker ID, victim ID, damage
                        break;
                    case "spawnBullet":
                        if(int.Parse(splitRawEvents[1]) != ID){
                            serverEvents.spawnBullet(splitRawEvents[1], splitRawEvents[2], splitRawEvents[3], splitRawEvents[4]); //senderID, position, rotation, travel speed
                        }
                        break;
                    case "sound":
                        //if(int.Parse(splitRawEvents[1]) != ID){
                            serverEvents.playSound(splitRawEvents[2], splitRawEvents[3], splitRawEvents[4], splitRawEvents[5]); //clipID, position, volume, pitcj
                        //}
                        break;
                    case "flash":
                        serverEvents.spawnFlash(splitRawEvents[2], splitRawEvents[3]); //position, velocity
                        break;
                    default:
                        Debug.LogError("Event called that doesn't have a function: " + splitRawEvents[0]);
                        Debug.Log("Message recieved: " + info);
                        break;
                }
            }
        }
    }
}
