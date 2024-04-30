using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : BaseBehaviour
{
    private NavMeshAgent enemyNavmesh; 

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemyNavmesh = animator.GetComponent<NavMeshAgent>();
        enemyNavmesh.isStopped = false;
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // isPreparingAttack
        float attackRange = enemy.attackRange;
        bool isAttacking = InRange(animator.transform, attackRange);
        animator.SetBool("isPreparingAttack", isAttacking);

        // needsHeal
        if (enemy.CurrentHealth <= 2)
            animator.SetBool("needsHeal", true);

        // Navmesh
        Move(animator);
    }

    private void Move(Animator animator)
    {
        enemyNavmesh.SetDestination(_player.position);
    }
}
