using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource source;

    

    public void playSound(AudioClip clip, float volume, float pitch){
        source.clip = clip;
        source.pitch = pitch;
        source.volume = volume;
        source.Play();
        Invoke("destroy", clip.length);
    }

    void destroy(){
        Destroy(this.gameObject);
    }
}
