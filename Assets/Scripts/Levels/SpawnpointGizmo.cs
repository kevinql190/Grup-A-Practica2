using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnpointGizmo : MonoBehaviour
{
    [SerializeField] private float radius = 1f;
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.4f);
        Gizmos.DrawSphere(transform.position, radius);
    }
}
