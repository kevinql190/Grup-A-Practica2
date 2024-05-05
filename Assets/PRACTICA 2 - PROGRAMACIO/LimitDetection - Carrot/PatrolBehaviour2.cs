using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour2 : BaseBehaviour
{
    protected float _timer;
    public float WaitTime = 3;

    // Variables para la detección de bordes
    public Transform CheckPoint;
    public float Speed = 2;
    public LayerMask WhatIsGround;
    public LayerMask WhatIsWall;
    public float WallDetectionDistance = 0.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
        CheckPoint = GameObject.FindGameObjectWithTag("CheckpointCarrot2").transform;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isChasing
        float enemyDetectionDistance = enemy.detectionDistance;
        if (InRange(animator.transform, enemyDetectionDistance) && CheckPlayerVisibility(animator))
        {
            animator.SetBool("isChasing", true);
        }

        // isPatroling
        bool time = CheckTime();
        animator.SetBool("isPatroling", !time);

        // Patrol
        if (NoGround() || DetectWall())
        {
            Turn(animator.transform);
        }

        Move(animator.transform);
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > WaitTime;
    }

    // Patrol
    private bool NoGround()
    {
        return !Physics.Raycast(CheckPoint.position, Vector3.down, 0.55f, WhatIsGround);
    }

    private bool DetectWall()
    {
        Vector3 rightDirection = CheckPoint.TransformDirection(Vector3.right);
        Vector3 leftDirection = CheckPoint.TransformDirection(Vector3.left);

        // Check for wall on the right side
        if (Physics.Raycast(CheckPoint.position, rightDirection, WallDetectionDistance, WhatIsWall))
            return true;

        // Check for wall on the left side
        if (Physics.Raycast(CheckPoint.position, leftDirection, WallDetectionDistance, WhatIsWall))
            return true;

        return false;
    }

    private void Turn(Transform transform)
    {
        float angle = Random.Range(90f, 270f);
        transform.Rotate(new Vector3(0, angle, 0));
    }

    private void Move(Transform transform)
    {
        transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }
}

