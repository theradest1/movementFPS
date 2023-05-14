using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableInfo : MonoBehaviour
{
    [Header("Controlled by server:")]
    public float damage;
    public float speed;
    public float cooldown;

    [Header("Settings:")]
    public int projectileID;
    public string title;
    public int throwSound;
    public float throwVolume;
    public float throwPitch;

    public void setVars(string[] vars){
        damage = float.Parse(vars[1]);
        speed = float.Parse(vars[2]);
        cooldown = float.Parse(vars[3]);
    }
}
