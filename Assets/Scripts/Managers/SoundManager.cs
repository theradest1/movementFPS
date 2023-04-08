using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public List<AudioClip> sounds;
    public GameObject audioSourcePrefab;
    public float generalVolume;

    public void playSound(int clipID, Vector3 position, float volume, float pitch, Transform parent = null){
        GameObject soundObject;
        if(parent != null){
            soundObject = Instantiate(audioSourcePrefab, position, Quaternion.identity, parent);
        }
        else{
            soundObject = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        }
        soundObject.GetComponent<SoundPlayer>().playSound(sounds[clipID], volume * generalVolume, pitch);
    }
}
