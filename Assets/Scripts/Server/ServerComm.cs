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
    int throughPackets = 0;
    int errorPackets = 0;
    public float updateSpeed;
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
        InvokeRepeating("serverUpdate", .1f, updateSpeed);
        InvokeRepeating("updatePPSGUI", 0f, 1f);
    }

    void updatePPSGUI(){
        Debug.Log("through packets: " + throughPackets);
        Debug.Log("error packets: " + errorPackets);

        throughPackets = 0;
        errorPackets = 0;
    }
    void Update() {
    }

    string serverUpdate()
    {
        try{
            //send
            byte[] sendBytes = Encoding.ASCII.GetBytes(player.transform.position + "~" + player.transform.rotation);
            client.Send(sendBytes, sendBytes.Length);
            
            //recieve
            byte[] receiveBytes = client.Receive(ref remoteEndPoint);

            throughPackets++;
            return Encoding.ASCII.GetString(receiveBytes);
        }
        catch(Exception e){
            Debug.LogError(e.Message);
            errorPackets++;
            return "";
        }
    }
}
