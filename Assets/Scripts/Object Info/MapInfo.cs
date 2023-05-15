using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapInfo : MonoBehaviour
{
    public List<GameObject> spawnPoints;
    public List<GameObject> toolSpawnPoints;

    [Header("Settings:")]
    public Material skyboxMaterial;
    public Cubemap skyboxCubemap;
    public Color sunColor;
    public float sunIntensity;
    public float overviewHeight;
}
