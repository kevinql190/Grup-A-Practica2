using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToHealBehaviour : BaseBehaviour
{
    private NavMeshAgent enemyNavmesh;
    private GameObject healObject;
    private bool reachedHealObject = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemyNavmesh = animator.GetComponent<NavMeshAgent>();

        // Go to heal
        healObject = GameObject.FindGameObjectWithTag("HealEnemy");
        enemyNavmesh.SetDestination(healObject.transform.position);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!reachedHealObject && enemyNavmesh.remainingDistance <= enemyNavmesh.stoppingDistance)
        {
            reachedHealObject = true;
            animator.SetBool("isHealing", true);
        }
    }
}
