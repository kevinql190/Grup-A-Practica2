using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolEdgeDetection : MonoBehaviour
{
    public Transform CheckPoint;
    public float Speed = 2;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsWall;
    public float WallDetectionDistance = 0.5f;

    void Update()
    {
        if (NoGround() || DetectWall())
            Turn();

        Move();
    }

    private bool NoGround()
    {
        return !Physics.Raycast(CheckPoint.position, Vector3.down, 0.55f, WhatIsGround);
    }

    private bool DetectWall()
    {
        Vector3 rightDirection = transform.TransformDirection(Vector3.right);
        Vector3 leftDirection = transform.TransformDirection(Vector3.left);

        // Check for wall on the right side
        if (Physics.Raycast(CheckPoint.position, rightDirection, WallDetectionDistance, WhatIsWall))
            return true;

        // Check for wall on the left side
        if (Physics.Raycast(CheckPoint.position, leftDirection, WallDetectionDistance, WhatIsWall))
            return true;

        return false;
    }

    private void Turn()
    {
        float angle = Random.Range(90f, 270f);
        transform.Rotate(new Vector3(0, angle, 0));
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}