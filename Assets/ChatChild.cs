using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatChild : MonoBehaviour
{
    public float timeUntilDeath;
    void Start()
    {
        Invoke("destroySelf", timeUntilDeath);
    }

    void destroySelf(){
        Destroy(this.gameObject);
    }
}
