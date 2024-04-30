using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
public class PlayerHeadIK : MonoBehaviour
{
    [SerializeField] AimConstraint aimConstraint;
    [SerializeField] GameObject targetObject;
    [SerializeField] float lookRadius;
    [SerializeField] private LayerMask _enemyLayer;
    private void Update()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, lookRadius, _enemyLayer);
        if (enemies.Length == 0) targetObject.transform.position = transform.position + transform.forward;
        else lookAtTarget(enemies);
    }
    private void lookAtTarget(Collider[] enemies)
    {
        float _closestDistance = Mathf.Infinity;
        Collider closestEnemy = new();
        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < _closestDistance)
            {
                closestEnemy = enemy;
                _closestDistance = distanceToEnemy;
            }
        }
        if (closestEnemy != null)
        {
            targetObject.transform.position = closestEnemy.transform.position;
        }
    }
}
