using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeVaulter : MonoBehaviour
{
    [Header("References:")]
    public ControlsManager controlsManager;
    public Rigidbody rb;

    [Header("Colliders:")]
    public float testersForwardDistance;
    public Vector3 upperScale;
    public Vector3 lowerScale;
    public float upperHeight;
    public float lowerHeight;
    public LayerMask vaultableLayers;


    [Header("Vault settings:")]
    public float upForce;
    public float forwardForce;

    private void FixedUpdate() {
        Collider[] upperColls = Physics.OverlapBox(transform.position + transform.forward * testersForwardDistance + Vector3.up * upperHeight, upperScale, transform.rotation, vaultableLayers);
        Collider[] lowerColls = Physics.OverlapBox(transform.position + transform.forward * testersForwardDistance + Vector3.up * lowerHeight, lowerScale, transform.rotation, vaultableLayers);
        if(upperColls.Length == 0 && lowerColls.Length > 0 && controlsManager.jumping){
            rb.velocity = new Vector3(rb.velocity.x, Mathf.Clamp(rb.velocity.y, 0, 99999f), rb.velocity.z);
            rb.AddForce(Vector3.up * upForce + transform.forward * forwardForce);
        }
    }

    private void OnDrawGizmos() {
        var oldMatrix = Gizmos.matrix;

        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * testersForwardDistance + Vector3.up * lowerHeight, transform.rotation, lowerScale * 2);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldMatrix;
        
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.forward * testersForwardDistance + Vector3.up * upperHeight, transform.rotation, upperScale * 2);
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        Gizmos.matrix = oldMatrix;
    }
}
