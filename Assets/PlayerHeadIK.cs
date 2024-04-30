using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class PlayerHeadIK : MonoBehaviour
{
    [SerializeField] GameObject targetConstraintCenter;
    [SerializeField] float lookRadius;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float sphereColliderDistance;
    [SerializeField] private Vector3 colliderSize;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private bool isGizmoActive;
    private void Update()
    {
        Collider[] enemies = Physics.OverlapBox(transform.position + transform.forward * sphereColliderDistance, colliderSize, Quaternion.identity, _enemyLayer);
        Quaternion lookAngle = enemies.Length == 0 ? Quaternion.LookRotation(transform.forward) : GetLookTarget(enemies);
        UpdateLookAt(lookAngle);
    }

    private void UpdateLookAt(Quaternion angle)
    {
        targetConstraintCenter.transform.rotation = Quaternion.Slerp(targetConstraintCenter.transform.rotation, angle, rotationSpeed * Time.deltaTime);
    }

    Quaternion GetLookTarget(Collider[] enemies)
    {
        Collider bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;
        foreach (Collider potentialTarget in enemies)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }
        return Quaternion.LookRotation(bestTarget.transform.position - transform.position);
    }
    private void OnDrawGizmos()
    {
        if (!isGizmoActive) return;
        Gizmos.DrawCube(transform.position + transform.forward * sphereColliderDistance, colliderSize);
    }
}
