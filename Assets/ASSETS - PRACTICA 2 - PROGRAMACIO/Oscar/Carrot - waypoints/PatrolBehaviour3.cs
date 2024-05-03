using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolBehaviour3 : BaseBehaviour
{
    protected float _timer;
    public float WaitTime = 3;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        _timer = 0;
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
    }
    protected bool CheckTime()
    {
        _timer += Time.deltaTime;
        return _timer > WaitTime;
    }
}

