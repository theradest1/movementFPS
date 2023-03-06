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
    
    [HideInInspector]
    public string username;

    ServerEvents serverEvents;
    GameObject player;
    TextMeshProUGUI PPSText;
    TextMeshProUGUI sendBPSText;
    TextMeshProUGUI recieveBPSText;
    TextMeshProUGUI latencyText;
    TextMeshProUGUI TPSText;
    ControlsManager controlsManager;

    int throughPackets = 0;
    int sendBPS;
    int recieveBPS;
    bool inSchool;

    VariableUpdater variableUpdater;

    public float updateSpeed;
    public int ID = -1;
    //public int CLIENTPORT;

    public float minUpdateSpeed;
    public float maxUpdateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        variableUpdater = GameObject.Find("manager").GetComponent<VariableUpdater>();
        controlsManager = GameObject.Find("manager").GetComponent<ControlsManager>();
        player = GameObject.Find("Player");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        PPSText = GameObject.Find("PPS debug").GetComponent<TextMeshProUGUI>();
        TPSText = GameObject.Find("TPS debug").GetComponent<TextMeshProUGUI>();
        sendBPSText = GameObject.Find("BPS send debug").GetComponent<TextMeshProUGUI>();
        recieveBPSText = GameObject.Find("BPS recieve debug").GetComponent<TextMeshProUGUI>();
        latencyText = GameObject.Find("Latency Debug").GetComponent<TextMeshProUGUI>();

        SERVERADDRESS = MainMenu.address;
        //CLIENTPORT = MainMenu.clientPort;
        SERVERPORT = MainMenu.port;
        username = MainMenu.username;
        
        try{
            client = new UdpClient(/*CLIENTPORT*/);
            client.Connect(SERVERADDRESS, SERVERPORT);
            remoteEndPoint = new IPEndPoint(IPAddress.Any, SERVERPORT);
        }
        catch(Exception e){
            Debug.LogError("Couldn't connect, exeption: " + e.Message);
        }

        join(username);
    }

    async void join(string name)
    {
        byte[] sendBytes = Encoding.ASCII.GetBytes("newClient~" + name);
        client.Send(sendBytes, sendBytes.Length);
        
        Debug.Log("Joining...");
        //recieve

        byte[] receiveBytes = new byte[0];
        await Task.Run(() => receiveBytes = client.Receive(ref remoteEndPoint));
        string recieveString = Encoding.ASCII.GetString(receiveBytes);
        ID = int.Parse(recieveString.Split('|')[0]);
        variableUpdater.updateVars(recieveString.Split('|')[1]);

        Debug.Log("User ID: " + ID);

        InvokeRepeating("updatePPSGUI", 1f, 1f);
        /*while(true){
            serverUpdate();
            await Task.Delay((int)(updateSpeed * 1000));
        }*/
    }

    void updatePPSGUI(){
        //Debug.Log("through packets: " + throughPackets);
        //Debug.Log("error packets: " + errorPackets);
        sendBPSText.text = "Send BPS: " + sendBPS;
        recieveBPSText.text = "Recieve BPS: " + recieveBPS;
        sendBPS = 0;
        recieveBPS = 0;
        PPSText.text = "PPS: " + throughPackets;
        TPSText.text = "TPS: " + Mathf.Round(1/updateSpeed);
        throughPackets = 0;

        CancelInvoke();
        InvokeRepeating("updatePPSGUI", 1f, 1f);
        InvokeRepeating("serverUpdate", 0f, updateSpeed);
    }

    public void send(string message){
        byte[] sendBytes = Encoding.ASCII.GetBytes(message);
        sendBPS += sendBytes.Length;
        client.Send(sendBytes, sendBytes.Length);
        throughPackets++;
    }

    async void serverUpdate()
    {
        string info = "";
        byte[] sendBytes;
        if(controlsManager.deathMenuControlls){
            sendBytes = Encoding.ASCII.GetBytes("u~" + ID + "~" + new Vector3(0f, -9, 0f) + "~" + player.transform.rotation);
        }
        else{
            sendBytes = Encoding.ASCII.GetBytes("u~" + ID + "~" + player.transform.position + "~" + player.transform.rotation);
        }
        sendBPS += sendBytes.Length;
        client.Send(sendBytes, sendBytes.Length);
        throughPackets++;

        //recieve
        byte[] receiveBytes = new byte[0];
        
        float latencyTimer = Time.time;
        await Task.Run(() => receiveBytes = client.Receive(ref remoteEndPoint));
        latencyText.text = "Latency: " + Mathf.Round((Time.time - latencyTimer) * 1000f);

        recieveBPS += receiveBytes.Length;
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
                    case "setClass":
                        if(int.Parse(splitRawEvents[1]) != ID){
                            serverEvents.setOtherClientClass(splitRawEvents[1], splitRawEvents[2]); //ID, class
                        }
                        break;
                    case "d": //damage
                        if(int.Parse(splitRawEvents[1]) != ID){
                            serverEvents.damage(splitRawEvents[2], splitRawEvents[3], splitRawEvents[4]); //attacker ID, victim ID, damage
                        }
                        break;
                    case "pr": //projectile
                        serverEvents.spawnProjectile(splitRawEvents[1], splitRawEvents[2], splitRawEvents[3], splitRawEvents[4], splitRawEvents[5]); //senderID, type ID, damage, position, velocity, sound ID, volume, pitch
                        serverEvents.playSound(splitRawEvents[6], splitRawEvents[4], splitRawEvents[7], splitRawEvents[8]);
                        break;
                    case "setHealth":
                        serverEvents.setHealth(splitRawEvents[1], splitRawEvents[2], splitRawEvents[3]); //clientID, health, healing cooldown
                        break;
                    case "s": //sound
                        serverEvents.playSound(splitRawEvents[2], splitRawEvents[3], splitRawEvents[4], splitRawEvents[5]); //clipID, position, volume, pitch
                        break;
                    case "death":
                        serverEvents.death(splitRawEvents[1], splitRawEvents[2]); //id of killer, id of killed
                        break;
                    case "updateSettings":
                        string things = "";
                        for(int j = 1; j < splitRawEvents.Length; j++){
                            things += splitRawEvents[j] + "~";
                        }
                        variableUpdater.updateVars(things.Substring(0, things.Length - 1));
                        break;
                    case "tps":
                        Debug.Log(splitRawEvents[1]);
                        updateSpeed = 1/float.Parse(splitRawEvents[1]);
                        break;
                    default:
                        Debug.LogError("Event called that doesn't have a function: " + splitRawEvents[0]);
                        Debug.Log("Message recieved: " + info);
                        break;
                }
            }
        }

        //Invoke("serverUpdate", Mathf.Clamp(updateSpeed - currentLatency, 0, 999999));
    }
}
