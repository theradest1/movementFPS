using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashBangScript : MonoBehaviour
{   
    GameObject playerCam;
    ServerEvents serverEvents;
    SoundManager soundManager;
    public float timeToFlash;
    public int bounceSound;
    public int throwSound;
    public int bangSound;

    // Start is called before the first frame update
    void Start()
    {
        playerCam = GameObject.Find("Main Camera");
        serverEvents = GameObject.Find("manager").GetComponent<ServerEvents>();
        soundManager = GameObject.Find("manager").GetComponent<SoundManager>();

        soundManager.playSound(throwSound, transform.position, 1f, 1.5f);
        //serverEvents.sendEvent("ue", "sound", throwSound + "~" + transform.position + "~1~1");
        Invoke("flash", timeToFlash);
    }

    void flash(){
        //Debug.Log("bang");
        soundManager.playSound(bangSound, transform.position, 1f, 1f);
        if(this.gameObject.GetComponent<Renderer>().isVisible && !Physics.Raycast(transform.position, playerCam.transform.position - transform.position, Vector3.Distance(transform.position, playerCam.transform.position))){
            GameObject.Find("flash image").GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        //serverEvents.sendEvent("ue", "sound", bangSound + "~" + transform.position + "~2~1");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll){
        soundManager.playSound(bounceSound, transform.position, 1f, 2f);
        //serverEvents.sendEvent("ue", "sound", bounceSound + "~" + transform.position + "~1~1");
    }
}
