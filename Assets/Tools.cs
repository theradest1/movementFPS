using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    public int equippedTool; //grapple, heal, double jump
    public GameObject grappleIndicator;
    public Grapple grapple;
    private void LateUpdate()
    {
        if(equippedTool == 0){
            grappleIndicator.transform.position = grapple.getLookPos();
        }
    }
}
