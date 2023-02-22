using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour
{
    public Rigidbody rb;
    public float timeToExplode;
    public int bangSound;
    public int bounceSound;
    SoundManager soundManager;
    Image flashImage;
    GameObject playerCam;

    public void setInfo(Vector3 givenVelocity, SoundManager givenSoundManager, Image givenFlashImage, GameObject givenPlayerCam){
        rb.velocity = givenVelocity;
        playerCam = givenPlayerCam;
        soundManager = givenSoundManager;
        flashImage = givenFlashImage;
        Invoke("explode", timeToExplode);
    }

    void explode(){
        soundManager.playSound(bangSound, transform.position, 1f, 1f);
        if(this.gameObject.GetComponent<Renderer>().isVisible && !Physics.Raycast(transform.position, playerCam.transform.position - transform.position, Vector3.Distance(transform.position, playerCam.transform.position))){
            flashImage.color = new Color(1, 1, 1, 1);
        }
        Debug.Log("bang (flash)");
        Destroy(this.gameObject);
    }

    void OnCollisionEnter(Collision coll){
        soundManager.playSound(bounceSound, transform.position, 1f, 2f);
    }
}
