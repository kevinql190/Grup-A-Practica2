using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    public float DetectionRange;
    public float FieldOfView;
    public LayerMask WhatIsOpaque;

    public Transform _player;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, DetectionRange);

        var direction = Quaternion.AngleAxis(FieldOfView / 2, transform.up) * transform.forward;
        Gizmos.DrawRay(transform.position, direction * DetectionRange);

        var direction2 = Quaternion.AngleAxis(-FieldOfView / 2, transform.up) * transform.forward;
        Gizmos.DrawRay(transform.position, direction2 * DetectionRange);

        Gizmos.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsPlayerClose())
        {

        }
    }

    private bool IsInFieldOfView()
    {
        Vector3 EP = _player.position - transform.position;
        float angle = Vector3.Angle(EP, transform.forward);

        return angle < FieldOfView / 2; 
    }

    private bool IsPlayerClose()
    {
        return Vector3.Distance(transform.position, _player.position) < DetectionRange;
    }
}