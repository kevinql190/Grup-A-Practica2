using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseBehaviour : BaseBehaviour
{
    private NavMeshAgent enemyNavmesh;
    // Extra IA - exclamation
    private bool firstTimeDetected = true;
    public GameObject exclamationPrefab;
    private Camera mainCamera;
    public GameObject exclamation;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        enemyNavmesh = animator.GetComponent<NavMeshAgent>();
        enemyNavmesh.isStopped = false;

        // Extra IA - exclamation
        mainCamera = Camera.main;
        if (firstTimeDetected && exclamationPrefab != null)
        {
            exclamation = Instantiate(exclamationPrefab, animator.gameObject.transform);
            exclamation.transform.localPosition = Vector3.up * 2f; 
            firstTimeDetected = false;
        }
    }
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Extra IA - Exclamation
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0f;
        exclamation.transform.rotation = Quaternion.LookRotation(cameraForward);

        // isPreparingAttack
        float attackRange = enemy.attackRange;
        bool isAttacking = InRange(animator.transform, attackRange);
        animator.SetBool("isPreparingAttack", isAttacking);

        // Navmesh
        Move(animator);
    }
    private void Move(Animator animator)
    {
        enemyNavmesh.SetDestination(_player.position);
    }

    // needsHeal
    private void OnEnable()
    {
        Enemy.OnEnemyHealthReached += HandleEnemyHealthReached;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyHealthReached -= HandleEnemyHealthReached;
    }
    private void HandleEnemyHealthReached(Animator animator)
    {
        animator.SetBool("needsHeal", true);
    }
}
