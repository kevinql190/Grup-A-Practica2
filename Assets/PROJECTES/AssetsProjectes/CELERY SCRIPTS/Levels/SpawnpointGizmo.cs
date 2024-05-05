using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointGizmo : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    [SerializeField] private bool isGizmoVisible = true;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.4f);
        if(isGizmoVisible) Gizmos.DrawSphere(transform.position, radius);
    }
}
