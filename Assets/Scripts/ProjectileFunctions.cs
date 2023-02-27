using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProjectileFunctions : MonoBehaviour
{
    //just a bunch of small functions to make projectile programming and other things faster and more readable
    //also has references to other scripts to stay clean
    public ServerComm serverComm;
    public ServerEvents serverEvents;
    public PlayerManager playerManager;
    public SoundManager soundManager;

    public GameObject playerCam;
    public Image flashImage;

    private void Start() {
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        serverComm = GameObject.Find("manager").GetComponent<ServerComm>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();
    }

    public void Explosion(Vector3 pos, float radius, float damage, float force, bool falloffDamage){

    }
}
