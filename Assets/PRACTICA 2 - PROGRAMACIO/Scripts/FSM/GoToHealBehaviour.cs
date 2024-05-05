using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToHealBehaviour : BaseBehaviour
{
    private NavMeshAgent enemyNavmesh;
    private GameObject healObject;
    private bool reachedHealObject = false;
    // Speed
    private float originalSpeed;
    public float speedGoToHeal = 3.0f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemyNavmesh = animator.GetComponent<NavMeshAgent>();
        originalSpeed = enemyNavmesh.speed;
        // Extra IA - modificar la velocidad del enemigo cuando se va a curar
        enemyNavmesh.speed = speedGoToHeal;

        // Go to heal
        reachedHealObject = false;
        healObject = GameObject.FindGameObjectWithTag("HealEnemy");
        enemyNavmesh.SetDestination(healObject.transform.position);
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!reachedHealObject && Vector3.Distance(enemyNavmesh.transform.position, healObject.transform.position) <= 3.1f)
        {
            reachedHealObject = true;
            // Original Speed
            enemyNavmesh.speed = originalSpeed;
            animator.SetBool("isHealing", true);
        }
        else if (!reachedHealObject)
        {
            enemyNavmesh.SetDestination(healObject.transform.position);
        }
    }
}
