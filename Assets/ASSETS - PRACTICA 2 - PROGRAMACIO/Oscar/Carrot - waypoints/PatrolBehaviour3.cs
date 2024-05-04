using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolBehaviour3 : BaseBehaviour
{
    protected float _timer;
    public float WaitTime = 3;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
        var firstTarget = GameObject.FindGameObjectWithTag("PathManagerWaypoints");
        enemy.target = firstTarget;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isChasing
        float enemyDetectionDistance = enemy.detectionDistance;
        if (InRange(animator.transform, enemyDetectionDistance) && CheckPlayerVisibility(animator))
        {
            animator.SetBool("isChasing", true);
        }

        // Patrolling
        enemy.GetComponent<NavMeshAgent>().SetDestination(new Vector3(enemy.target.transform.position.x, animator.transform.position.y, enemy.target.transform.position.z));
    }
}

