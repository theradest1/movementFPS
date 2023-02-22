using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    
    public List<AudioClip> sounds;
    public GameObject audioSourcePrefab;

    public void playSound(int clipID, Vector3 position, float volume, float pitch){
        GameObject soundObject = Instantiate(audioSourcePrefab, position, Quaternion.identity);
        soundObject.GetComponent<SoundPlayer>().playSound(sounds[clipID], volume, pitch);
    }
}
